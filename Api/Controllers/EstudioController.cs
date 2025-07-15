using Application.DTO.Estudio;
using Application.Services.Estudio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EstudioController : ControllerBase
{
    private readonly IEstudioService _estudioService;

    public EstudioController(IEstudioService estudioService)
    {
        _estudioService = estudioService;
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("criar-estudio")]
    public async Task<IActionResult> CriarEstudio([FromBody] EstudioCreateDTO dto)
    {
        var resposta =  await _estudioService.CreateAsync(dto);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return Ok(resposta);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("editar-estudio/{id}")]
    public async Task<IActionResult> EditarEstudio(int id,[FromBody] EstudioUpdateDTO dto)
    {
        var resposta = await _estudioService.UpdateAsync(id,dto);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return  Ok(resposta);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("delete-estudio/{id}")]
    public async Task<IActionResult> DeleteEstudio(int id)
    {
        var resposta = await _estudioService.DeleteAsync(id);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return Ok(resposta);
    }

    [HttpGet]
    [Route("estudio/{id}")]
    public async Task<IActionResult> ObterEstudioId(int id)
    {
        var resposta = await _estudioService.GetByIdAsync(id);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }

        return Ok(resposta);
    }

    [HttpGet]
    [Route("todos-estudios")]
    public async Task<IActionResult> TodosEstudios()
    {
        var resposta = await _estudioService.GetAllAsync();
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return Ok(resposta);
    }
    
}