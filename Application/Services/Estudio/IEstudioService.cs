using Application.DTO.Estudio;

namespace Application.Services.Estudio;

public interface IEstudioService
{
    public Task<IEnumerable<EstudioDTO>> GetAllAsync();
    public Task<EstudioDTO> GetByIdAsync(int id);
    public Task<EstudioDTO> CreateAsync(EstudioCreateDTO dto);
    public Task UpdateAsync(int id, EstudioUpdateDTO dto);
    public Task DeleteAsync(int id);





}