using Application.DTO.Anime;
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

    public async Task<Response<string>> UpdateAsync(int id, AvaliacaoUpdateDTO dto, int usuarioId)
    {
        var resposta = new Response<string>();

        var avaliacao = await _unityOfWork.Avaliacoes.GetById(id);
        if (avaliacao == null)
        {
            resposta.Status = false;
            resposta.Mensagem = "Avaliação não encontrada";
            return resposta;
        }

        if (avaliacao.UsuarioId != usuarioId)
        {
            resposta.Status = false;
            resposta.Mensagem = "Você não tem permissão para editar esta avaliação";
            return resposta;
        }

        avaliacao.Nota = dto.Nota;
        avaliacao.Comentario = dto.Comentario;

        _unityOfWork.Avaliacoes.Update(avaliacao);
        await _unityOfWork.SaveChangesAsync();
        
        await AtualizarMediaDoAnime(avaliacao.AnimeId);

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

    public async Task<Response<IEnumerable<AvaliacaoDTO>>> GetAvaliacoesPorUsuario(int usuarioId)
    {
        var resposta = new Response<IEnumerable<AvaliacaoDTO>>();
        
        var avaliacoes = await _unityOfWork.Avaliacoes.FindAsync(a =>a.UsuarioId == usuarioId);
        
        resposta.Status = true;
        resposta.Mensagem = "Avaliações do usuário carregadas";
        resposta.Dados = avaliacoes.Select(a => new AvaliacaoDTO()
        {
            Id = a.Id,
            Nota = a.Nota,
            Comentario = a.Comentario,
            UsuarioId = a.UsuarioId,
            AnimeId = a.AnimeId
        });
        return resposta;
    }

    public async Task<Response<IEnumerable<AvaliacaoDTO>>> GetAvaliacoesPorAnime(int animeId)
    {
        var resposta = new Response<IEnumerable<AvaliacaoDTO>>();
        
        var avaliacoes = await _unityOfWork.Avaliacoes.FindAsync(a => a.AnimeId == animeId);
        
        resposta.Status = true;
        resposta.Mensagem = "Avalições do anime carregadas";
        resposta.Dados = avaliacoes.Select(a => new AvaliacaoDTO()
        {
            Id = a.Id,
            Nota = a.Nota,
            AnimeId = a.AnimeId,
            Comentario = a.Comentario,
            UsuarioId = a.UsuarioId
        });
        return resposta;
    }

    public async Task<Response<IEnumerable<AnimeDTO>>> GetMaisBemAvaliados(int quantidade = 10)
    {
        var resposta = new Response<IEnumerable<AnimeDTO>>();

        var animes = await _unityOfWork.Animes.GetAll();
        var topAnimes = animes
            .OrderByDescending(a => a.MediaNota)
            .Take(quantidade)
            .Select(a => new AnimeDTO
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Descricao = a.Descricao,
                MediaNota = a.MediaNota
            });
        
        resposta.Status = true;
        resposta.Mensagem = "Animes mais bem avaliados carregados";
        resposta.Dados = topAnimes;

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
