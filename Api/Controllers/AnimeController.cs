using Application.DTO.Anime;
using Application.Services.Anime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimeController : ControllerBase
{
    private readonly IAnimeService _animeService;

    public AnimeController(IAnimeService animeService)
    {
        _animeService = animeService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("criar-anime")]
    public async Task<IActionResult> CriarAnime([FromBody] AnimeCreateDTO dto)
    {
        var resposta = await _animeService.CreateAsync(dto);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return Ok(resposta);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("editar-anime/{id}")]
    public async Task<IActionResult> EditarAnime(int id,[FromBody] AnimeUpdateDTO dto)
    {
        var resposta = await _animeService.UpdateAsync(id,dto);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return  Ok(resposta);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("delete-anime/{id}")]
    public async Task<IActionResult> DeleteAnime(int id)
    {
        var resposta = await _animeService.DeleteAsync(id);
        if (!resposta.Status)
        {
            return BadRequest(resposta);
        }
        return Ok(resposta);
    }
}