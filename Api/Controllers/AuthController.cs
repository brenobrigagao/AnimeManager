using System.Security.Claims;
using Application.DTO.Auth;
using Application.DTO.Token;
using Application.DTO.Usuario;
using Application.Services.Auth;
using Application.Services.Senha;
using Infra.Data.Context;
using Infra.Entities;
using Infra.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly AppDbContext _context;

    public AuthController(IAuthService authService, AppDbContext context,ISenhaService senhaService)
    {
        _authService = authService;
        _context = context;
    }

    [HttpPost("Registrar")]
    public async Task<IActionResult> Registrar(UsuarioCreateDTO dto)
    {
        var resposta = await _authService.Registrar(dto);
        return Ok(resposta);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UsuarioLoginDTO dto)
    {
        var resposta = await _authService.Login(dto);
        return Ok(resposta);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO request)
    {
        var resultado = await _authService.RefreshToken(request);
        if (!resultado.Status)
        {
            return BadRequest();
        }
        return Ok(resultado);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenRequestDTO request)
    {
        var resultado = await _authService.Logout(request);
        if (!resultado.Status)
        {
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }

    [Authorize]
    [HttpGet("Logout Global")]
    public async Task<IActionResult> LogoutGlobal()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("ID do usuário não foi encontrado no token");
        }
        int usuarioId = int.Parse(userId.Value);
        var resultado = await _authService.LogoutGlobal(usuarioId);
        if (!resultado.Status)
        {
            return BadRequest(resultado);
        }
        return Ok(resultado);
    }
}

