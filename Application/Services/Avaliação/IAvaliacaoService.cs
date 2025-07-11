using Application.DTO.Avaliacao;
using Infra.Entities;

namespace Application.Services.Avaliação;

public interface IAvaliacaoService
{
    public Task<Response<IEnumerable<AvaliacaoDTO>>> GetAllAsync();
    public Task<Response<AvaliacaoDTO>> GetByIdAsync(int id);
    public Task<Response<AvaliacaoDTO>> CreateAsync(AvaliacaoCreateDTO dto);
    public Task<Response<string>> UpdateAsync(int id, AvaliacaoUpdateDTO dto);
    public Task<Response<string>> DeleteAsync(int id);





}