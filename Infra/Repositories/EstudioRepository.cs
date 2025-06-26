using Infra.Data.Context;
using Infra.Entities;

namespace Infra.Repositories;

public class EstudioRepository : Repository<Estudio>, IEstudioRepository
{
    public EstudioRepository(AppDbContext context) : base(context) {}
}