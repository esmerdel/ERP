using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ObterTodosAsync();
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task RemoverAsync(Usuario usuario);
    }
}
