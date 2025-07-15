using Application.DTO.Anime;
using Infra.Repositories;
using Infra.Entities;

namespace Application.Services.Anime;

public class AnimeService : IAnimeService
{
    private readonly UnityOfWork _unityOfWork;

    public AnimeService(UnityOfWork unityOfWork)
    {
        _unityOfWork = unityOfWork;
    }

    public async Task<Response<AnimeDTO>> GetByIdAsync(int id)
    {
        var resposta = new Response<AnimeDTO>();

        var anime = await _unityOfWork.Animes.GetById(id);
        if (anime == null)
        {
            resposta.Status = false;
            resposta.Mensagem = "Anime não encontrado";
            return resposta;
        }

        resposta.Status = true;
        resposta.Mensagem = "Anime encontrado";
        resposta.Dados = new AnimeDTO
        {
            Id = anime.Id,
            Descricao = anime.Descricao,
            Titulo = anime.Titulo
        };

        return resposta;
    }

    public async Task<Response<IEnumerable<AnimeDTO>>> GetAllAsync()
    {
        var resposta = new Response<IEnumerable<AnimeDTO>>();

        var animes = await _unityOfWork.Animes.GetAll();

        resposta.Status = true;
        resposta.Mensagem = "Lista de animes carregada com sucesso";
        resposta.Dados = animes.Select(a => new AnimeDTO
        {
            Id = a.Id,
            Descricao = a.Descricao,
            Titulo = a.Titulo
        });

        return resposta;
    }

    public async Task<Response<AnimeDTO>> CreateAsync(AnimeCreateDTO dto)
    {
        var resposta = new Response<AnimeDTO>();

        var existente = await _unityOfWork.Animes.FindAsync(a =>
            a.Titulo == dto.Titulo &&
            a.Ano == dto.Ano &&
            a.EstudioId == dto.EstudioId &&
            a.GeneroId == dto.GeneroId);

        if (existente.Any())
        {
            resposta.Status = false;
            resposta.Mensagem = "Já existe um anime com esse gênero para esse estúdio no mesmo ano";
            return resposta;
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

        resposta.Status = true;
        resposta.Mensagem = "Anime criado com sucesso";
        resposta.Dados = new AnimeDTO
        {
            Id = anime.Id,
            Titulo = anime.Titulo,
            Descricao = anime.Descricao
        };

        return resposta;
    }

    public async Task<Response<string>> UpdateAsync(int id, AnimeUpdateDTO dto)
    {
        var resposta = new Response<string>();

        var anime = await _unityOfWork.Animes.GetById(id);
        if (anime == null)
        {
            resposta.Status = false;
            resposta.Mensagem = "Esse anime não foi encontrado";
            return resposta;
        }

        anime.Titulo = dto.Titulo;
        anime.Descricao = dto.Descricao;

        _unityOfWork.Animes.Update(anime);
        await _unityOfWork.SaveChangesAsync();

        resposta.Status = true;
        resposta.Mensagem = "Anime atualizado com sucesso";
        resposta.Dados = "Atualização concluída";

        return resposta;
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var resposta = new Response<string>();

        var anime = await _unityOfWork.Animes.GetById(id);
        if (anime == null)
        {
            resposta.Status = false;
            resposta.Mensagem = "Esse anime não existe";
            return resposta;
        }

        _unityOfWork.Animes.Delete(anime);
        await _unityOfWork.SaveChangesAsync();

        resposta.Status = true;
        resposta.Mensagem = "Anime removido com sucesso";
        resposta.Dados = "Deleção concluída";

        return resposta;
    }

    public async Task<Response<IEnumerable<AnimeDTO>>> FiltrarAsync(string? titulo, int? generoId, int? estudioId)
    {
        var resposta = new Response<IEnumerable<AnimeDTO>>();
        var animes = await _unityOfWork.Animes.FindAsync(a =>
            (string.IsNullOrEmpty(titulo) || a.Titulo.Contains(titulo)) && 
            (!generoId.HasValue || a.GeneroId == generoId.Value) && 
            (!estudioId.HasValue || a.EstudioId == estudioId.Value));
        
        resposta.Mensagem = "Animes filtrados com sucesso!";
        resposta.Status = true;
        resposta.Dados = animes.Select(a => new AnimeDTO
        {
            Id = a.Id,
            Titulo = a.Titulo,
            Descricao = a.Descricao
        });
        return resposta;
    }
}
