using Infra.Entities;

namespace Infra.Repositories.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario> GetByEmail(string email);
}