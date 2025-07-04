using Application.DTO.Auth;
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
    public async Task<Response<string>> Login(UsuarioLoginDTO dto)
    {
        Response<string> respostaServico = new Response<string>();
        try
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
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

            var token = _senhaService.CriarToken(usuario);
            respostaServico.Mensagem = "Usuario logado com sucesso";
            respostaServico.Dados = token;
            
        }
        catch (Exception ex)
        {
            respostaServico.Dados = null;
            respostaServico.Mensagem = ex.Message;
            respostaServico.Status = false;
        }
        return respostaServico;
    }

}