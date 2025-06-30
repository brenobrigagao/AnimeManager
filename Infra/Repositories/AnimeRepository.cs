using Infra.Data.Context;
using Infra.Entities;
using Infra.Repositories.Interfaces;

namespace Infra.Repositories;

public class AnimeRepository : Repository<Anime>, IAnimeRepository
{
    public AnimeRepository(AppDbContext context) : base(context) {}
}