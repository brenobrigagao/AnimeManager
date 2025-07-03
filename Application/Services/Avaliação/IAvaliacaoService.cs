using Application.DTO.Avaliacao;

namespace Application.Services.Avaliação;

public interface IAvaliacaoService
{
    public Task<IEnumerable<AvaliacaoDTO>> GetAllAsync();
    public Task<AvaliacaoDTO> GetByIdAsync(int id);
    public Task<AvaliacaoDTO> CreateAsync(AvaliacaoCreateDTO dto);
    public Task UpdateAsync(int id, AvaliacaoUpdateDTO dto);
    public Task DeleteAsync(int id);





}