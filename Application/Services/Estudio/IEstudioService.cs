using Application.DTO.Estudio;
using Infra.Entities;

namespace Application.Services.Estudio
{
    public interface IEstudioService
    {
        Task<Response<EstudioDTO>> GetByIdAsync(int id);
        Task<Response<IEnumerable<EstudioDTO>>> GetAllAsync();
        Task<Response<EstudioDTO>> CreateAsync(EstudioCreateDTO dto);
        Task<Response<string>> UpdateAsync(int id, EstudioUpdateDTO dto);
        Task<Response<string>> DeleteAsync(int id);
    }
}