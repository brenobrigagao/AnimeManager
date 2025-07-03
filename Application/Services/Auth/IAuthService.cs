using Application.DTO.Auth;
using Application.DTO.Usuario;
using Infra.Entities;

namespace Application.Services.Auth;

public interface IAuthService
{
    public Task<Response<UsuarioCreateDTO>> Registrar(UsuarioCreateDTO usuario);
    public bool VerificaUsuarioEmail(UsuarioCreateDTO usuarioRegistro);
    public Task<Response<string>> Login(UsuarioLoginDTO dto);

}