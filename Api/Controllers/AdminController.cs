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
    private readonly IAdminService _adminService;
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
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

    [HttpGet("listar-usuarios")]
    public async Task<ActionResult> ListarUsuarios()
    {
        var resultado = await _adminService.ListarUsuarios();
        if(!resultado.Status)
        {
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }

    [Authorize]
    [HttpDelete("deletar-usuario/{id}")]
    public async Task<ActionResult> DeletarUsuario(int id)
    {
        var resultado = await _adminService.DeletarUsuario(id);
        if(!resultado.Status)
        {
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }
}