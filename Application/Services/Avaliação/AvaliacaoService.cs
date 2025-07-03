using Application.DTO.Avaliacao;
using Application.Services.Avaliação;
using Infra.Repositories;
using Infra.Entities;

namespace Application.Services.Avaliacao;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly UnityOfWork _unityOfWork;

    public AvaliacaoService(UnityOfWork unityOfWork)
    {
        _unityOfWork = unityOfWork;
    }

    public async Task<IEnumerable<AvaliacaoDTO>> GetAllAsync()
    {
        var avaliacoes = await _unityOfWork.Avaliacoes.GetAll();
        return avaliacoes.Select(a => new AvaliacaoDTO
        {
            Id = a.Id,
            Nota = a.Nota,
            Comentario = a.Comentario,
            UsuarioId = a.UsuarioId,
            AnimeId = a.AnimeId
        });
    }

    public async Task<AvaliacaoDTO> GetByIdAsync(int id)
    {
        var avaliacao = await _unityOfWork.Avaliacoes.GetById(id);
        if (avaliacao == null) throw new Exception("Avaliação não encontrada");

        return new AvaliacaoDTO
        {
            Id = avaliacao.Id,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
            UsuarioId = avaliacao.UsuarioId,
            AnimeId = avaliacao.AnimeId
        };
    }

    public async Task<AvaliacaoDTO> CreateAsync(AvaliacaoCreateDTO dto)
    {
        var nova = new Infra.Entities.Avaliacao
        {
            Nota = dto.Nota,
            Comentario = dto.Comentario,
            UsuarioId = dto.UsuarioId,
            AnimeId = dto.AnimeId
        };

        await _unityOfWork.Avaliacoes.Add(nova);
        await _unityOfWork.SaveChangesAsync();

        return new AvaliacaoDTO
        {
            Id = nova.Id,
            Nota = nova.Nota,
            Comentario = nova.Comentario,
            UsuarioId = nova.UsuarioId,
            AnimeId = nova.AnimeId
        };
    }

    public async Task UpdateAsync(int id, AvaliacaoUpdateDTO dto)
    {
        var avaliacao = await _unityOfWork.Avaliacoes.GetById(id);
        if (avaliacao == null) throw new Exception("Avaliação não encontrada");

        avaliacao.Nota = dto.Nota;
        avaliacao.Comentario = dto.Comentario;

        _unityOfWork.Avaliacoes.Update(avaliacao);
        await _unityOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var avaliacao = await _unityOfWork.Avaliacoes.GetById(id);
        if (avaliacao == null) throw new Exception("Avaliação não encontrada");

        _unityOfWork.Avaliacoes.Delete(avaliacao);
        await _unityOfWork.SaveChangesAsync();
    }
}
