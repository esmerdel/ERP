using System.Security.Cryptography;
using System.Text;
using ERP.Application.Services;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

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

        // =============================
        //  CADASTRO DE USUÁRIO
        // =============================
        public async Task<string> RegistrarAsync(string nome, string email, string senha, int empresaId)
        {
            if (await _usuarioRepository.EmailExisteAsync(email))
                throw new Exception("E-mail já cadastrado.");

            var senhaHash = GerarHashSenha(senha);

            var usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                SenhaHash = senhaHash,
                EmpresaId = empresaId
            };

            await _usuarioRepository.AdicionarAsync(usuario);

            return "Usuário cadastrado com sucesso.";
        }

        // =============================
        //  LOGIN / AUTENTICAÇÃO
        // =============================
        public async Task<string> LoginAsync(string email, string senha)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            if (!VerificarSenha(senha, usuario.SenhaHash))
                throw new Exception("Senha incorreta.");

            var token = _tokenService.GerarToken(usuario);
            return token;
        }

        // =============================
        //  MÉTODOS AUXILIARES
        // =============================
        private string GerarHashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerificarSenha(string senhaDigitada, string senhaHash)
        {
            var hashDigitado = GerarHashSenha(senhaDigitada);
            return hashDigitado == senhaHash;
        }
    }
}
