using Application.DTO.Usuario;

namespace Application.Services.Usuario;

public interface IUsuarioService
{
    Task<UsuarioDTO> GetByIdAsync(int id);
    Task<IEnumerable<UsuarioDTO>> GetAllAsync();
    Task<UsuarioDTO> CreateAsync(UsuarioCreateDTO dto);
    Task<UsuarioDTO> UpdateAsync(int id,UsuarioUpdateDTO dto);
    Task<UsuarioDTO> DeleteAsync(int id);
}