using Application.DTO.Anime;

namespace Application.Services.Anime;

public interface IAnimeService
{
    Task<AnimeDTO> GetByIdAsync(int id);
    Task<IEnumerable<AnimeDTO>> GetAllAsync();
    Task<AnimeDTO> CreateAsync(AnimeCreateDTO dto);
    Task UpdateAsync(int id, AnimeUpdateDTO dto);
    Task DeleteAsync(int id);
}