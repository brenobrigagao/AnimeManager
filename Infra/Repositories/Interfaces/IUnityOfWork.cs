using Infra.Entities;

namespace Infra.Repositories.Interfaces;

public interface IUnityOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    IRepository<Anime> Animes { get; }
    IRepository<Avaliacao> Avaliacoes { get; }
    IRepository<Estudio> Estudios { get; }
    IRepository<Genero> Generos { get; }
    Task<int> SaveChangesAsync();
}