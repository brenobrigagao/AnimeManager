using Application.DTO.Anime;
using Infra.Entities; 

namespace Application.Services.Anime
{
    public interface IAnimeService
    {
        Task<Response<AnimeDTO>> GetByIdAsync(int id);
        Task<Response<IEnumerable<AnimeDTO>>> GetAllAsync();
        Task<Response<AnimeDTO>> CreateAsync(AnimeCreateDTO dto);
        Task<Response<string>> UpdateAsync(int id, AnimeUpdateDTO dto);
        Task<Response<string>> DeleteAsync(int id);
    }
}