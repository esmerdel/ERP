using ERP.Application.Services;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // 🔹 GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            return Ok(usuarios);
        }

        // 🔹 GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return Ok(usuario);
        }

        // 🔹 POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _usuarioService.AdicionarAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        // 🔹 PUT: api/usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
                return BadRequest("ID informado não confere com o usuário.");

            var atualizado = await _usuarioService.AtualizarAsync(usuario);

            if (!atualizado)
                return NotFound("Usuário não encontrado.");

            return NoContent();
        }

        // 🔹 DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _usuarioService.RemoverAsync(id);

            if (!removido)
                return NotFound("Usuário não encontrado.");

            return NoContent();
        }
    }
}
