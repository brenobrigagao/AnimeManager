using Application.DTO.Usuario;
using Infra.Entities;

namespace Application.Services.Admin;

public interface IAdminService
{
    public Task<Response<UsuarioDTO>> RegistrarAdmin(UsuarioCreateDTO dto);
    public Task<Response<string>> PromoverAdmin(int id);


}