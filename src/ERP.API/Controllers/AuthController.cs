using ERP.Application.Services;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // =============================
        //  ENDPOINT: CADASTRAR USUÁRIO
        // =============================
        [HttpPost("register")]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequest request)
        {
            try
            {
                var resultado = await _usuarioService.RegistrarAsync(
                    request.Nome,
                    request.Email,
                    request.Senha,
                    request.EmpresaId
                );

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // =============================
        //  ENDPOINT: LOGIN
        // =============================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _usuarioService.LoginAsync(request.Email, request.Senha);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { erro = ex.Message });
            }
        }
    }

    // =============================
    //  DTOs (ViewModels)
    // =============================

    public class RegisterRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int EmpresaId { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
