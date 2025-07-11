using Application.DTO.Usuario;
using Infra.Entities;

namespace Application.Services.Usuario;

public interface IUsuarioService
{
    public Task<Response<UsuarioDTO>> GetByIdAsync(int id);
    public Task<Response<IEnumerable<UsuarioDTO>>> GetAllAsync();
    public Task<Response<UsuarioDTO>> CreateAsync(UsuarioCreateDTO dto);
    public Task<Response<string>> UpdateAsync(int id, UsuarioUpdateDTO dto);
    public Task<Response<string>> DeleteAsync(int id);
    public Task<Response<UsuarioDTO>> CreateAdminAsync(UsuarioCreateDTO dto);
    public Task<Response<string>> UpdateAdminAsync(int id);


}