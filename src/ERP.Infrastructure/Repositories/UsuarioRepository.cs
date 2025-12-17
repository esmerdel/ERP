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

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }
    }
}
