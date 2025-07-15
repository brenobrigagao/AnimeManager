using Application.DTO.Avaliacao;
using Application.Services.Avaliação;
using Infra.Entities;
using Infra.Repositories;

namespace Application.Services.Avaliacao;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly UnityOfWork _unityOfWork;

    public AvaliacaoService(UnityOfWork unityOfWork)
    {
        _unityOfWork = unityOfWork;
    }

    public async Task<Response<IEnumerable<AvaliacaoDTO>>> GetAllAsync()
    {
        var resposta = new Response<IEnumerable<AvaliacaoDTO>>();

        var avaliacoes = await _unityOfWork.Avaliacoes.GetAll();

        resposta.Status = true;
        resposta.Mensagem = "Lista de avaliações carregada com sucesso";
        resposta.Dados = avaliacoes.Select(a => new AvaliacaoDTO
        {
            Id = a.Id,
            Nota = a.Nota,
            Comentario = a.Comentario,
            UsuarioId = a.UsuarioId,
            AnimeId = a.AnimeId
        });

        return resposta;
    }

    public async Task<Response<AvaliacaoDTO>> GetByIdAsync(int id)
    {
        var resposta = new Response<AvaliacaoDTO>();

        var avaliacao = await _unityOfWork.Avaliacoes.GetById(id);
        if (avaliacao == null)
        {
            resposta.Status = false;
            resposta.Mensagem = "Avaliação não encontrada";
            return resposta;
        }

        resposta.Status = true;
        resposta.Mensagem = "Avaliação encontrada";
        resposta.Dados = new AvaliacaoDTO
        {
            Id = avaliacao.Id,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
            UsuarioId = avaliacao.UsuarioId,
            AnimeId = avaliacao.AnimeId
        };

        return resposta;
    }

    public async Task<Response<AvaliacaoDTO>> CreateAsync(AvaliacaoCreateDTO dto)
    {
        var resposta = new Response<AvaliacaoDTO>();
        
        var existente = await _unityOfWork.Avaliacoes.FindAsync(a =>
            a.UsuarioId == dto.UsuarioId && a.AnimeId == dto.AnimeId);

        if (existente.Any())
        {
            resposta.Status = false;
            resposta.Mensagem = "Você já avaliou esse anime";
            return resposta;
        }
        var nova = new Infra.Entities.Avaliacao
        {
            Nota = dto.Nota,
            Comentario = dto.Comentario,
            UsuarioId = dto.UsuarioId,
            AnimeId = dto.AnimeId
        };

        await _unityOfWork.Avaliacoes.Add(nova);
        await _unityOfWork.SaveChangesAsync();

        resposta.Status = true;
        resposta.Mensagem = "Avaliação criada com sucesso";
        resposta.Dados = new AvaliacaoDTO
        {
            Id = nova.Id,
            Nota = nova.Nota,
            Comentario = nova.Comentario,
            UsuarioId = nova.UsuarioId,
            AnimeId = nova.AnimeId
        };
        await AtualizarMediaDoAnime(dto.AnimeId);
        return resposta;
    }

    public async Task<Response<string>> UpdateAsync(int id, AvaliacaoUpdateDTO dto)
    {
        var resposta = new Response<string>();

        var avaliacao = await _unityOfWork.Avaliacoes.GetById(id);
        if (avaliacao == null)
        {
            resposta.Status = false;
            resposta.Mensagem = "Avaliação não encontrada";
            return resposta;
        }

        avaliacao.Nota = dto.Nota;
        avaliacao.Comentario = dto.Comentario;

        _unityOfWork.Avaliacoes.Update(avaliacao);
        await _unityOfWork.SaveChangesAsync();

        resposta.Status = true;
        resposta.Mensagem = "Avaliação atualizada com sucesso";
        resposta.Dados = "Atualização concluída";

        return resposta;
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var resposta = new Response<string>();

        var avaliacao = await _unityOfWork.Avaliacoes.GetById(id);
        if (avaliacao == null)
        {
            resposta.Status = false;
            resposta.Mensagem = "Avaliação não encontrada";
            return resposta;
        }

        _unityOfWork.Avaliacoes.Delete(avaliacao);
        await _unityOfWork.SaveChangesAsync();

        resposta.Status = true;
        resposta.Mensagem = "Avaliação deletada com sucesso";
        resposta.Dados = "Deleção concluída";

        return resposta;
    }

    private async Task AtualizarMediaDoAnime(int animeId)
    {
        var avaliacoes = await _unityOfWork.Avaliacoes.FindAsync(a => a.AnimeId == animeId);
        var media = avaliacoes.Any() ? avaliacoes.Average(a => a.Nota) : 0;

        var anime = await _unityOfWork.Animes.GetById(animeId);
        if (anime != null)
        {
            anime.MediaNota = media;  
            _unityOfWork.Animes.Update(anime);
            await _unityOfWork.SaveChangesAsync();
        }
    }

}
