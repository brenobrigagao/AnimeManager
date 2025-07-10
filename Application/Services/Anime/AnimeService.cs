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
        try
        {
            var anime = await _unityOfWork.Animes.GetById(id);
            if (anime == null)
                throw new KeyNotFoundException("Anime não encontrado");

            resposta.Status = true;
            resposta.Mensagem = "Anime encontrado";
            resposta.Dados = new AnimeDTO
            {
                Id = anime.Id,
                Descricao = anime.Descricao,
                Titulo = anime.Titulo
            };
        }
        catch (Exception ex)
        {
            resposta.Status = false;
            resposta.Mensagem = $"Erro ao buscar anime: {ex.Message}";
        }
        return resposta;
    }

    public async Task<Response<IEnumerable<AnimeDTO>>> GetAllAsync()
    {
        var resposta = new Response<IEnumerable<AnimeDTO>>();
        try
        {
            var animes = await _unityOfWork.Animes.GetAll();
            resposta.Status = true;
            resposta.Mensagem = "Lista de animes carregada com sucesso";
            resposta.Dados = animes.Select(a => new AnimeDTO
            {
                Id = a.Id,
                Descricao = a.Descricao,
                Titulo = a.Titulo
            });
        }
        catch (Exception ex)
        {
            resposta.Status = false;
            resposta.Mensagem = $"Erro ao listar animes: {ex.Message}";
            resposta.Dados = null;
        }
        return resposta;
    }

    public async Task<Response<AnimeDTO>> CreateAsync(AnimeCreateDTO dto)
    {
        var resposta = new Response<AnimeDTO>();
        try
        {
            var existente = await _unityOfWork.Animes.FindAsync(a =>
                a.Titulo == dto.Titulo &&
                a.Ano == dto.Ano &&
                a.EstudioId == dto.EstudioId &&
                a.GeneroId == dto.GeneroId);

            if (existente.Any())
                throw new Exception("Já existe um anime com esse gênero para esse estúdio no mesmo ano");

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
        }
        catch (Exception ex)
        {
            resposta.Status = false;
            resposta.Mensagem = $"Erro ao criar anime: {ex.Message}";
        }
        return resposta;
    }

    public async Task<Response<string>> UpdateAsync(int id, AnimeUpdateDTO dto)
    {
        var resposta = new Response<string>();
        try
        {
            var anime = await _unityOfWork.Animes.GetById(id);
            if (anime == null)
                throw new Exception("Esse anime não foi encontrado");

            anime.Titulo = dto.Titulo;
            anime.Descricao = dto.Descricao;

            _unityOfWork.Animes.Update(anime);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Anime atualizado com sucesso";
            resposta.Dados = "Atualização concluída";
        }
        catch (Exception ex)
        {
            resposta.Status = false;
            resposta.Mensagem = $"Erro ao atualizar anime: {ex.Message}";
        }
        return resposta;
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var resposta = new Response<string>();
        try
        {
            var anime = await _unityOfWork.Animes.GetById(id);
            if (anime == null)
                throw new Exception("Esse anime não existe");

            _unityOfWork.Animes.Delete(anime);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Anime removido com sucesso";
            resposta.Dados = "Deleção concluída";
        }
        catch (Exception ex)
        {
            resposta.Status = false;
            resposta.Mensagem = $"Erro ao deletar anime: {ex.Message}";
        }
        return resposta;
    }
}
