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
        var claim = User.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value;
        return claim == "True";
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
        var resposta = new Response<UsuarioDTO>
        {
            Mensagem = "Admin registrado com sucesso.",
            Dados = novoAdmin,
            Status = true
        };
        return Ok(resposta);

    }
    
}