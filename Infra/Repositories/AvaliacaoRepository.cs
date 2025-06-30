using Infra.Data.Context;
using Infra.Entities;
using Infra.Repositories.Interfaces;

namespace Infra.Repositories;

public class AvaliacaoRepository : Repository<Avaliacao>, IAvaliacaoRepository
{
    public AvaliacaoRepository(AppDbContext context) : base(context) {}
}