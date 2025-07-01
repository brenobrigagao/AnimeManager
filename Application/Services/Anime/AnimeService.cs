using Application.DTO.Anime;
using Infra.Repositories;

namespace Application.Services.Anime;

public class AnimeService : IAnimeService
{
    private readonly UnityOfWork _unityOfWork;
    public AnimeService(UnityOfWork unityOfWork)
    {
        _unityOfWork = unityOfWork;
    }

    public async Task<AnimeDTO> GetByIdAsync(int id)
    {
        var anime = await _unityOfWork.Animes.GetById(id);
        if (anime == null) throw new KeyNotFoundException("Anime não encontrado");
        return new AnimeDTO()
        {
            Id = anime.Id,
            Descricao = anime.Descricao,
            Titulo = anime.Titulo
        };
    }
    public async Task<IEnumerable<AnimeDTO>> GetAllAsync()
    {
        var animes = await _unityOfWork.Animes.GetAll();
        return animes.Select(u => new AnimeDTO
            {
                Id = u.Id,
                Descricao = u.Descricao,
                Titulo = u.Titulo
            }
        );
    }
    public async Task<AnimeDTO> CreateAsync(AnimeCreateDTO dto)
    {
        var existente = await _unityOfWork.Animes.FindAsync(a => a.Titulo == dto.Titulo && a.Ano == dto.Ano && a.EstudioId == dto.EstudioId && a.GeneroId == 
            dto.GeneroId);
        if (existente.Any())
        {
            throw new Exception("Já existe um anime com esse genero para esse estudio no mesmo ano");
        }
        var anime = new Infra.Entities.Anime
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Ano = dto.Ano,
            EstudioId = dto.EstudioId,
            GeneroId = dto.GeneroId
        };
        await _unityOfWork.Animes.Add(anime);
        await _unityOfWork.SaveChangesAsync();
        return new AnimeDTO()
        {
            Id = anime.Id,
            Titulo = anime.Titulo,
            Descricao = anime.Descricao
        };
    }

    public async Task UpdateAsync(int id, AnimeUpdateDTO dto)
    {
        var anime = await _unityOfWork.Animes.GetById(id);
        if (anime == null)
        {
            throw new Exception("Esse anime não foi encontrado");
        }
        anime.Titulo = dto.Titulo;
        anime.Descricao = dto.Descricao;
        _unityOfWork.Animes.Update(anime);
        await _unityOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var anime = await _unityOfWork.Animes.GetById(id);
        if (anime == null)
        {
            throw new Exception("Esse anime não existe");
        }
        _unityOfWork.Animes.Delete(anime);
        await _unityOfWork.SaveChangesAsync();
    }
}