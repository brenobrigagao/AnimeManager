using Infra.Data.Context;
using Infra.Entities;
using Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) {}

    public async Task<Usuario> GetByEmail(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }
}