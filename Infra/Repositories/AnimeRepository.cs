using Infra.Data.Context;
using Infra.Entities;

namespace Infra.Repositories;

public class AnimeRepository : Repository<Anime>, IAnimeRepository
{
    public AnimeRepository(AppDbContext context) : base(context) {}
}