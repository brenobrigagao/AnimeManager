using Application.DTO.Auth;
using Application.DTO.Token;
using Application.DTO.Usuario;
using Application.Services.Auth;
using Application.Services.Senha;
using Infra.Data.Context;
using Infra.Entities;
using Infra.Repositories.Interfaces;
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
}

