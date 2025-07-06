using Application.DTO.Auth;
using Application.DTO.Token;
using Application.DTO.Usuario;
using Application.Services.Senha;
using Infra.Data.Context;
using Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly ISenhaService _senhaService;
    public AuthService(AppDbContext context, ISenhaService senhaService)
    {
        _context = context;
        _senhaService = senhaService;
    }
    
    public async Task<Response<UsuarioCreateDTO>> Registrar(UsuarioCreateDTO usuarioRegistro)
    {
        Response<UsuarioCreateDTO> respostaServico = new Response<UsuarioCreateDTO>();

        try
        {
            if (!VerificaUsuarioEmail(usuarioRegistro))
            {
                respostaServico.Dados = null;
                respostaServico.Status = false;
                respostaServico.Mensagem = "Email/Usuario já cadastrado";
                return respostaServico;
            }
            _senhaService.CriarHashSenha(usuarioRegistro.Senha, out byte[] hash, out byte[] salt);
            Infra.Entities.Usuario usuario = new Infra.Entities.Usuario()
            {
                Nome = usuarioRegistro.Nome,
                Email = usuarioRegistro.Email,
                SenhaHash = hash,
                SenhaSalt = salt,
                IsAdmin = false
            };
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            respostaServico.Mensagem = "Usuario criado com sucesso!";


        }
        catch (Exception e)
        {
            respostaServico.Dados = null;
            respostaServico.Mensagem = e.Message;
            respostaServico.Status = false;
        }

        return respostaServico;
    }

    public bool VerificaUsuarioEmail(UsuarioCreateDTO usuarioRegistro)
    {
        var usuario = _context.Usuarios.FirstOrDefault(ub => ub.Email == usuarioRegistro.Email || ub.Nome == usuarioRegistro.Nome );
        if (usuario != null) return false;
        return true;
    }
    public async Task<Response<TokenDTO>> Login(UsuarioLoginDTO dto)
    {
        Response<TokenDTO> respostaServico = new Response<TokenDTO>();
        try
        {
            var usuario = await _context.Usuarios.Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null)
            {
                respostaServico.Mensagem = "Esse email não está cadastrado";
                respostaServico.Status = false;
                return respostaServico;
            }
            if(!_senhaService.VerificaSenhaHash(dto.Senha,usuario.SenhaHash, usuario.SenhaSalt))
            {
                respostaServico.Mensagem = "Senha incorreta";
                respostaServico.Status = false;
                return respostaServico;
            }

            var acessToken = _senhaService.CriarToken(usuario);
            var refreshToken = _senhaService.GerarRefreshToken();

            var refreshTokenEntity = new RefreshToken()
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                UsuarioId = usuario.Id,
                Revogado = false
            };
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            respostaServico.Mensagem = "Usuario logado com sucesso";
            respostaServico.Status = true;
            respostaServico.Dados = new TokenDTO
            {
                AcessToken = acessToken,
                RefreshToken = refreshToken
            };
        }
        catch (Exception ex)
        {
            respostaServico.Dados = null;
            respostaServico.Mensagem = ex.Message;
            respostaServico.Status = false;
        }
        return respostaServico;
    }

    public async Task<Response<TokenDTO>> RefreshToken(RefreshTokenRequestDTO request)
    {
        Response<TokenDTO> respostaServico = new Response<TokenDTO>();

        try
        {
            var refreshTokenEntity = await _context.RefreshTokens
                .Include(rt => rt.Usuario)
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            if (refreshTokenEntity == null || refreshTokenEntity.Revogado ||
                refreshTokenEntity.Expires <= DateTime.UtcNow)
            {
                respostaServico.Mensagem = "Refresh Token inválido ou expirado";
                respostaServico.Status = false;
                return respostaServico;
            }

            var novoAcessToken = _senhaService.CriarToken(refreshTokenEntity.Usuario);
            var novoRefreshToken = _senhaService.GerarRefreshToken();

            refreshTokenEntity.Revogado = true;
            _context.RefreshTokens.Add(new RefreshToken
            {
                Token = novoAcessToken,
                Expires = DateTime.UtcNow.AddDays(7),
                UsuarioId = refreshTokenEntity.Usuario.Id,
                Revogado = false
            });
            await _context.SaveChangesAsync();

            respostaServico.Mensagem = "Token renovado com sucesso";
            respostaServico.Status = true;
            respostaServico.Dados = new TokenDTO
            {
                AcessToken = novoAcessToken,
                RefreshToken = novoRefreshToken
            };
        }
        catch (Exception ex)
        {
            respostaServico.Mensagem = ex.Message;
            respostaServico.Status = false;
            respostaServico.Dados = null;
        }

        return respostaServico;
    }

}

