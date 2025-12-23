using ERP.Application.Services;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // ==========================================
        // 📌 GET: api/Cliente
        // Retorna todos os clientes
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientes = await _clienteService.ObterTodosAsync();
            return Ok(clientes);
        }

        // ==========================================
        // 📌 GET: api/Cliente/{id}
        // Retorna um cliente por ID
        // ==========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            return Ok(cliente);
        }

        // ==========================================
        // 📌 POST: api/Cliente
        // Adiciona um novo cliente
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            try
            {
                var mensagem = await _clienteService.AdicionarAsync(cliente);
                return Ok(new { mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // ==========================================
        // 📌 PUT: api/Cliente/{id}
        // Atualiza um cliente existente
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest(new { mensagem = "ID informado não confere com o cliente enviado." });

            var atualizado = await _clienteService.AtualizarAsync(cliente);

            if (!atualizado)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            return Ok(new { mensagem = "Cliente atualizado com sucesso!" });
        }

        // ==========================================
        // 📌 DELETE: api/Cliente/{id}
        // Remove (soft delete) um cliente
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _clienteService.RemoverAsync(id);

            if (!removido)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            return Ok(new { mensagem = "Cliente removido com sucesso!" });
        }
    }
}
