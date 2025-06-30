using Application.DTO.Usuario;

namespace Application.Services.Usuario;

public interface IUsuarioService
{
    Task<UsuarioDTO> GetByIdAsync(int id);
    Task<IEnumerable<UsuarioDTO>> GetAllAsync();
    Task<UsuarioDTO> CreateAsync(UsuarioCreateDTO dto);
    Task UpdateAsync(int id, UsuarioUpdateDTO dto);
    Task DeleteAsync(int id);
}