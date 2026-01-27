using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly ERPContext _context;

        public VendaRepository(ERPContext context)
        {
            _context = context;
        }

        public async Task<Venda?> GetByIdAsync(int id)
        {
            return await _context.Vendas
                .Include(v => v.Itens)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Venda>> GetAllAsync()
        {
            return await _context.Vendas
                .Include(v => v.Itens)
                .ToListAsync();
        }

        public async Task AddAsync(Venda venda)
        {
            await _context.Vendas.AddAsync(venda);
            await _context.SaveChangesAsync();
        }
    }
}
