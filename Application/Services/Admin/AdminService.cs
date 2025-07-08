using System.Security.Claims;
using Application.DTO.Usuario;
using Application.Services.Usuario;
using Infra.Data.Context;
using Infra.Entities;

namespace Application.Services.Admin;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;
    private readonly IUsuarioService _usuarioService;

    public AdminService(AppDbContext context,IUsuarioService usuarioService)
    {
        _context = context;
        _usuarioService = usuarioService;
    }

    public async Task<Response<UsuarioDTO>> RegistrarAdmin(UsuarioCreateDTO dto)
    {
        var respostaServico = new Response<UsuarioDTO>();
        try
        {
            var novoAdmin = await _usuarioService.CreateAdminAsync(dto);
            respostaServico.Status = true;
            respostaServico.Mensagem = "Admin cadastrado com sucesso";
            respostaServico.Dados = novoAdmin;
        }
        catch (Exception e)
        {
            respostaServico.Status = false;
            respostaServico.Mensagem = $"Erro ao cadastrar um novo admin: {e.Message}";
            respostaServico.Dados = null;
        }
        return respostaServico;
    }

    public async Task<Response<string>> PromoverAdmin(int id)
    {
        var respostaServico = new Response<string>();
        try
        {
            await _usuarioService.UpdateAdminAsync(id);
            respostaServico.Status = true;
            respostaServico.Mensagem = "Admin promovido com sucesso";
        }
        catch (Exception e)
        {
            respostaServico.Status = false;
            respostaServico.Mensagem = $"Erro ao promover um admin: {e.Message}";
        }
        return respostaServico;
    }
}