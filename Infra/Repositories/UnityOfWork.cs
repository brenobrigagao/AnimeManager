using Infra.Data.Context;
using Infra.Entities;
using Infra.Repositories.Interfaces;

namespace Infra.Repositories;

public class UnityOfWork : IUnityOfWork
{
    private readonly AppDbContext _context;
    public IRepository<Anime> Animes { get; }
    public IRepository<Avaliacao> Avaliacoes { get; }
    public IRepository<Estudio> Estudios { get; }
    public IRepository<Genero> Generos { get; }
    public IUsuarioRepository Usuarios { get; }
    public UnityOfWork(AppDbContext context)
    {
        _context = context;
        Animes = new Repository<Anime>(context);
        Avaliacoes = new Repository<Avaliacao>(context);
        Estudios = new Repository<Estudio>(context);
        Generos = new Repository<Genero>(context);
        Usuarios = new UsuarioRepository(context);
    }
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}