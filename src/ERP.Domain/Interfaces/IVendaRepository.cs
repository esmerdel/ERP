using ERP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Domain.Interfaces
{
    public interface IVendaRepository
    {
        Task<Venda?> GetByIdAsync(int id);
        Task<IEnumerable<Venda>> GetAllAsync();
        Task AddAsync(Venda venda);
    }
}
