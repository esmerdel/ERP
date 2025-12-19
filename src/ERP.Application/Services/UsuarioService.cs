using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using BCrypt.Net;

namespace ERP.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;

        public UsuarioService(IUsuarioRepository usuarioRepository, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        // =====================================================
        // 🧩 SEÇÃO 1: CRUD DE USUÁRIOS
        // =====================================================

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            return await _usuarioRepository.ObterTodosAsync();
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _usuarioRepository.ObterPorIdAsync(id);
        }

        public async Task<string> AdicionarAsync(Usuario usuario)
        {
            var existente = await _usuarioRepository.ObterPorEmailAsync(usuario.Email);
            if (existente != null)
                return "Já existe um usuário com este e-mail.";

            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
            await _usuarioRepository.AdicionarAsync(usuario);
            return "Usuário adicionado com sucesso!";
        }

        public async Task<bool> AtualizarAsync(Usuario usuario)
        {
            var existente = await _usuarioRepository.ObterPorIdAsync(usuario.Id);
            if (existente == null)
                return false;

            existente.Nome = usuario.Nome;
            existente.Email = usuario.Email;
            existente.EmpresaId = usuario.EmpresaId;

            await _usuarioRepository.AtualizarAsync(existente);
            return true;
        }


        public async Task<bool> RemoverAsync(int id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null)
                return false;

            await _usuarioRepository.RemoverAsync(usuario);
            return true;
        }



        // =====================================================
        // 🔐 SEÇÃO 2: AUTENTICAÇÃO (REGISTRO E LOGIN)
        // =====================================================

        public async Task<string> RegistrarAsync(string nome, string email, string senha, int empresaId)
        {
            // Verifica se já existe um usuário com o mesmo e-mail
            var existente = await _usuarioRepository.ObterPorEmailAsync(email);
            if (existente != null)
                return "Usuário já cadastrado com este e-mail.";

            // Cria o novo usuário
            var usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
                EmpresaId = empresaId
            };

            await _usuarioRepository.AdicionarAsync(usuario);
            return "Usuário registrado com sucesso!";
        }

        public async Task<string?> LoginAsync(string email, string senha)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
                return null;

            // Gera o token JWT
            var token = _tokenService.GerarToken(usuario);
            return token;
        }
    }
}
