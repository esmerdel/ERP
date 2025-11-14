using BCrypt.Net;
using ERP.API.ViewModels.Auth;
using ERP.Application.Services;
using ERP.Domain.Entities;
using ERP.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ERPContext _context;
        private readonly TokenService _tokenService;

        public AuthController(ERPContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // Verifica se já existe empresa com o mesmo CNPJ
            if (await _context.Empresas.AnyAsync(e => e.Cnpj == request.Cnpj))
                return BadRequest("Empresa já cadastrada.");

            var empresa = new Empresa
            {
                Nome = request.NomeEmpresa,
                Cnpj = request.Cnpj
            };

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            var usuario = new Usuario
            {
                Nome = request.NomeUsuario,
                Email = request.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                EmpresaId = empresa.Id,
                Role = "Admin"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(usuario);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                return Unauthorized("Credenciais inválidas.");

            var token = _tokenService.GenerateToken(usuario);
            return Ok(new { token });
        }
    }
}
