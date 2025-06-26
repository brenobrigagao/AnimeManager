using Infra.Entities;

namespace Infra.Repositories;

public interface IUnityOfWork : IDisposable
{
    IRepository<Usuario> Usuarios { get; }
    IRepository<Anime> Animes { get; }
    IRepository<Avaliacao> Avaliacoes { get; }
    IRepository<Estudio> Estudios { get; }
    IRepository<Genero> Generos { get; }
    Task<int> SaveChangesAsync();
}