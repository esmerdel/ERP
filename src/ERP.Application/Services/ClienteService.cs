using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        // =====================================================
        // 🧩 CRUD DE CLIENTES
        // =====================================================

        // 1️⃣ Listar todos os clientes
        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
        {
            return await _clienteRepository.ObterTodosAsync();
        }

        // 2️⃣ Buscar cliente por ID
        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            return await _clienteRepository.ObterPorIdAsync(id);
        }

        // 3️⃣ Adicionar novo cliente
        public async Task<string> AdicionarAsync(Cliente cliente)
        {
            // Exemplo de validação simples
            var existente = await _clienteRepository.ObterPorEmailAsync(cliente.Email);
            if (existente != null)
                return "Já existe um cliente com este e-mail.";

            await _clienteRepository.AdicionarAsync(cliente);
            return "Cliente cadastrado com sucesso!";
        }

        // 4️⃣ Atualizar cliente
        public async Task<bool> AtualizarAsync(Cliente cliente)
        {
            var existente = await _clienteRepository.ObterPorIdAsync(cliente.Id);
            if (existente == null)
                return false;

            existente.Nome = cliente.Nome;
            existente.Email = cliente.Email;
            existente.Telefone = cliente.Telefone;
            existente.Endereco = cliente.Endereco;
            existente.DefinirEmpresa(cliente.EmpresaId);


            await _clienteRepository.AtualizarAsync(existente);
            return true;
        }

        // 5️⃣ Remover cliente
        public async Task<bool> RemoverAsync(int id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id);
            if (cliente == null)
                return false;

            await _clienteRepository.RemoverAsync(cliente);
            return true;
        }
    }
}
