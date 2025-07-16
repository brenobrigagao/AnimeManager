using Application.DTO.Anime;
using Application.DTO.Avaliacao;
using Infra.Entities;

namespace Application.Services.Avaliação;

public interface IAvaliacaoService
{
    public Task<Response<IEnumerable<AvaliacaoDTO>>> GetAllAsync();
    public Task<Response<AvaliacaoDTO>> GetByIdAsync(int id);
    public Task<Response<AvaliacaoDTO>> CreateAsync(AvaliacaoCreateDTO dto);
    public Task<Response<string>> UpdateAsync(int id, AvaliacaoUpdateDTO dto, int usuarioId);
    public Task<Response<string>> DeleteAsync(int id);
    public Task<Response<IEnumerable<AnimeDTO>>> GetMaisBemAvaliados(int quantidade = 10);
    public Task<Response<IEnumerable<AvaliacaoDTO>>> GetAvaliacoesPorAnime(int animeId);
    public Task<Response<IEnumerable<AvaliacaoDTO>>> GetAvaliacoesPorUsuario(int usuarioId);
}