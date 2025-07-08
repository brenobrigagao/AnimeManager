using System.Security.Claims;
using Application.DTO.Usuario;
using Application.Services.Usuario;
using Infra.Entities;
using Infra.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IUsuarioService _usuarioService;


    private bool UsuarioEhAdmin()
    {
        var claim = User.Claims.FirstOrDefault(c =>
            c.Type == "IsAdmin" || c.Type == ClaimTypes.Role && c.Value == "Admin");
        if (claim == null)
        {
            return false;
        }
        return claim.Value.Equals("True", StringComparison.InvariantCultureIgnoreCase) 
            || claim.Value.Equals("Admin", StringComparison.InvariantCultureIgnoreCase);
    }

    [Authorize]
    [HttpPost("registrar-admin")]
    public async Task<ActionResult<Response<UsuarioDTO>>> RegistrarAdminAsync(UsuarioCreateDTO dto)
    {
        if (!UsuarioEhAdmin())
        {
            return Forbid();
        }
        var novoAdmin = await _usuarioService.CreateAdminAsync(dto);
        
        return Ok(new Response<UsuarioDTO>
            {
                Mensagem = "Admin cadastrado com sucesso",
                Dados =  novoAdmin,
                Status = true
            }
        );
    }

    [Authorize]
    [HttpPost("promover-admin/{id}")]
    public async Task<ActionResult<Response<string>>> PromoverAdminAsync(int id)
    {
        if (!UsuarioEhAdmin())
        {
            return Forbid();
        }

        await _usuarioService.UpdateAdminAsync(id);
        return Ok(new Response<Response<string>>
        {
            Mensagem = "Usuario promovido com sucesso",
            Dados = null,
            Status = true
        });
    }

    [Authorize]
    [HttpGet("usuarios")]
    public async Task<ActionResult<Response<IEnumerable<UsuarioDTO>>>> ListarUsuarios()
    {
        if (!UsuarioEhAdmin()) return Forbid();
        
        var usuarios = await _usuarioService.GetAllAsync();
        return Ok(new Response<IEnumerable<UsuarioDTO>>
        {
            Mensagem = "Aqui estão todos os usuários",
            Dados = usuarios,
            Status = true
        });
    }

    [Authorize]
    [HttpGet("usuario/{id}")]
    public async Task<ActionResult<Response<string>>> DeletarUsuario(int id)
    {
        if(!UsuarioEhAdmin()) return  Forbid();
        await _usuarioService.DeleteAsync(id);
        return Ok(new Response<string>
        {
            Mensagem = "Usuario deletado com sucesso",
            Dados = null,
            Status = true
        });
    }
    
    
    
}