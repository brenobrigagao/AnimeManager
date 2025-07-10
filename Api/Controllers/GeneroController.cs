using Application.DTO.Genero;
using Application.Services.Genero;
using Infra.Entities;
using Infra.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GeneroController : ControllerBase
{
    private readonly IGeneroService _generoService;

    public GeneroController(IGeneroService generoService)
    {
        _generoService = generoService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("criar-genero")]
    public async Task<IActionResult> CriarGenero([FromBody] GeneroCreateDTO dto)
    {
       var resposta =  await _generoService.CreateAsync(dto);
       if (!resposta.Status)
       {
           return BadRequest(resposta);
       }
       return Ok(resposta);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("editar-genero/{id}")]
    public async Task<IActionResult> EditarGenero(int id,[FromBody] GeneroUpdateDTO dto)
    {
        var resposta = await _generoService.UpdateAsync(id,dto);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return  Ok(resposta);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("delete-genero/{id}")]
    public async Task<IActionResult> DeleteGenero(int id)
    {
        var resposta = await _generoService.DeleteAsync(id);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return Ok(resposta);
    }
    
    
}