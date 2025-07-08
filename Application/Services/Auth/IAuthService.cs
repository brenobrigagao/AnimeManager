using Application.DTO.Auth;
using Application.DTO.Token;
using Application.DTO.Usuario;
using Infra.Entities;

namespace Application.Services.Auth;

public interface IAuthService
{
    public Task<Response<UsuarioCreateDTO>> Registrar(UsuarioCreateDTO usuario);
    public bool VerificaUsuarioEmail(UsuarioCreateDTO usuarioRegistro);
    public Task<Response<TokenDTO>> Login(UsuarioLoginDTO dto);
    public Task<Response<TokenDTO>> RefreshToken(RefreshTokenRequestDTO request);
    public Task<Response<string>> Logout(RefreshTokenRequestDTO request);
    public Task<Response<string>> LogoutGlobal(int UsuarioId);
    public Task<Response<string>> SolicitarRefefinicaoSenha(EsqueciSenhaDTO dto);
    public Task<Response<string>> ResetarSenha(ResetarSenhaDTO dto);






}