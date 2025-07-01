using Application.DTO.Usuario;
using Application.Services.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _service;
    public UsuarioController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioDTO>>> GetAll()
    {
        var usuarios = await _service.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _service.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound("O usuario não encontrado");
        }
        return Ok(usuario);
    }
    [HttpPost]
    public async Task<IActionResult> Create(UsuarioCreateDTO dto)
    {
        var usuario = await _service.CreateAsync(dto);
        return CreatedAtAction("Usuario criado com sucesso",usuario);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UsuarioUpdateDTO dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
    
}