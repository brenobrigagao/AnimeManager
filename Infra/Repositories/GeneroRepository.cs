using Infra.Data.Context;
using Infra.Entities;
using Infra.Repositories.Interfaces;

namespace Infra.Repositories;

public class GeneroRepository : Repository<Genero>, IGeneroRepository
{
    public GeneroRepository(AppDbContext context) : base(context){}
}