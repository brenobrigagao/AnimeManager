using Application.DTO.Genero;

namespace Application.Services.Genero;

public interface IGeneroService
{
    public Task<IEnumerable<GeneroDTO>> GetAllAsync();
    public Task<GeneroDTO> GetByIdAsync(int id);
    public Task<GeneroDTO> CreateAsync(GeneroCreateDTO dto);
    public Task UpdateAsync(int id, GeneroUpdateDTO dto);
    public Task DeleteAsync(int id);





}