using Application.DTO.Genero;
using Infra.Entities;

namespace Application.Services.Genero
{
    public interface IGeneroService
    {
        Task<Response<GeneroDTO>> GetByIdAsync(int id);
        Task<Response<IEnumerable<GeneroDTO>>> GetAllAsync();
        Task<Response<GeneroDTO>> CreateAsync(GeneroCreateDTO dto);
        Task<Response<string>> UpdateAsync(int id, GeneroUpdateDTO dto);
        Task<Response<string>> DeleteAsync(int id);
    }
}