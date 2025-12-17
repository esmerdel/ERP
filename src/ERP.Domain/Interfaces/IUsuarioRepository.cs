using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task AdicionarAsync(Usuario usuario);
        Task<bool> EmailExisteAsync(string email);
    }
}
