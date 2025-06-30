using Infra.Data.Context;
using Infra.Entities;
using Infra.Repositories.Interfaces;

namespace Infra.Repositories;

public class EstudioRepository : Repository<Estudio>, IEstudioRepository
{
    public EstudioRepository(AppDbContext context) : base(context) {}
}