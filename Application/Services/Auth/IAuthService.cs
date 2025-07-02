using Application.DTO.Usuario;
using Infra.Entities;

namespace Application.Services.Auth;

public interface IAuthService
{
    public async Task<Response<UsuarioCreateDTO>> Registrar(UsuarioCreateDTO usuario);
    public bool VerificaUsuarioEmail(UsuarioCreateDTO usuarioRegistro);
}