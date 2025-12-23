using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<Cliente?> ObterPorEmailAsync(string email);
    }
}
