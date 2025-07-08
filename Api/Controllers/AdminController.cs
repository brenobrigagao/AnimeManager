using System.Security.Claims;
using Application.DTO.Usuario;
using Application.Services.Admin;
using Application.Services.Usuario;
using Infra.Entities;
using Infra.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly IAdminService _adminService;
    private readonly IUsuarioService _usuarioService;

    public AdminController(IUnityOfWork unityOfWork, IUsuarioService usuarioService, IAdminService adminService)
    {
        _unityOfWork = unityOfWork;
        _usuarioService = usuarioService;
        _adminService = adminService;
    }
    
    [Authorize]
    [HttpPost("registrar-admin")]
    public async Task<ActionResult> RegistrarAdminAsync(UsuarioCreateDTO dto)
    {
        var resultado = await _adminService.RegistrarAdmin(dto);
        if(!resultado.Status)
        {
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }

    [Authorize]
    [HttpPost("promover-admin/{id}")]
    public async Task<ActionResult> PromoverAdminAsync(int id)
    {
      var resultado = await _adminService.PromoverAdmin(id);
      if(!resultado.Status)
      {
          return BadRequest(resultado);
      }
      return Ok(resultado);
    }

    [Authorize]
    [HttpGet("usuarios")]
    public async Task<ActionResult<Response<IEnumerable<UsuarioDTO>>>> ListarUsuarios()
    {
        
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
        await _usuarioService.DeleteAsync(id);
        return Ok(new Response<string>
        {
            Mensagem = "Usuario deletado com sucesso",
            Dados = null,
            Status = true
        });
    }
    
    
    
}