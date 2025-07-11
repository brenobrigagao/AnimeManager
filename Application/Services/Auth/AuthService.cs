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
        var respostaServico = new Response<UsuarioCreateDTO>();

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
        respostaServico.Status = true;
        respostaServico.Dados = usuarioRegistro;

        return respostaServico;
    }

    public bool VerificaUsuarioEmail(UsuarioCreateDTO usuarioRegistro)
    {
        var usuario = _context.Usuarios.FirstOrDefault(ub => ub.Email == usuarioRegistro.Email || ub.Nome == usuarioRegistro.Nome );
        return usuario == null;
    }
    
    public async Task<Response<TokenDTO>> Login(UsuarioLoginDTO dto)
    {
        var respostaServico = new Response<TokenDTO>();

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

        return respostaServico;
    }

    public async Task<Response<TokenDTO>> RefreshToken(RefreshTokenRequestDTO request)
    {
        var respostaServico = new Response<TokenDTO>();

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
            Token = novoRefreshToken,
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

        return respostaServico;
    }

    public async Task<Response<string>> Logout(RefreshTokenRequestDTO request)
    {
        var respostaServico = new Response<string>();

        var token = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
        if (token == null || token.Revogado)
        {
            respostaServico.Mensagem = "Token inválido ou já revogado";
            respostaServico.Status = false;
            respostaServico.Dados = null;
            return respostaServico;
        }

        token.Revogado = true;
        await _context.SaveChangesAsync();

        respostaServico.Status = true;
        respostaServico.Mensagem = "Logout realizado com sucesso";

        return respostaServico;
    }

    public async Task<Response<string>> LogoutGlobal(int UsuarioId)
    {
        var respostaServico = new Response<string>();

        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UsuarioId == UsuarioId && !rt.Revogado)
            .ToListAsync();
        foreach (var token in tokens)
        {
            token.Revogado = true;
        }

        await _context.SaveChangesAsync();
        respostaServico.Status = true;
        respostaServico.Mensagem = "Todos os RefreshToken foram revogados com sucesso";

        return respostaServico;
    }

    public async Task<Response<string>> SolicitarRefefinicaoSenha(EsqueciSenhaDTO dto)
    {
        var respostaServico = new Response<string>();

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (usuario == null)
        {
            respostaServico.Mensagem = "Email não cadastrado";
            respostaServico.Status = false;
            return respostaServico;
        }
        var token = Guid.NewGuid().ToString();
        var resetToken = new PasswordResetToken
        {
            Token = token,
            Expires = DateTime.UtcNow.AddMinutes(30),
            usuarioId = usuario.Id,
            Usado = false
        };
        _context.PasswordResetTokens.Add(resetToken);
        await _context.SaveChangesAsync();
        respostaServico.Mensagem = "Token de redefinição enviado com sucesso";
        respostaServico.Status = true;
        respostaServico.Dados = token;

        return respostaServico;
    }

    public async Task<Response<string>> ResetarSenha(ResetarSenhaDTO dto)
    {
        var respostaServico = new Response<string>();

        var resetToken = await _context.PasswordResetTokens.FirstOrDefaultAsync(t =>
            t.Token == dto.Token && !t.Usado && t.Expires > DateTime.UtcNow);
        if (resetToken == null)
        {
            respostaServico.Mensagem = "Token inválido ou expirado";
            respostaServico.Status = false;
            return respostaServico;
        }

        _senhaService.CriarHashSenha(dto.NovaSenha, out byte[] hash, out byte[] salt);
        resetToken.Usuario.SenhaHash = hash;
        resetToken.Usuario.SenhaSalt = salt;
        resetToken.Usado = true;
        await _context.SaveChangesAsync();

        respostaServico.Status = true;
        respostaServico.Mensagem = "Senha resetada com sucesso";

        return respostaServico;
    }
}
