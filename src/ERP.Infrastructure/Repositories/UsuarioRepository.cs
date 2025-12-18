using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ERPContext _context;

        public UsuarioRepository(ERPContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            return await _context.Usuarios.AsNoTracking().ToListAsync();
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
