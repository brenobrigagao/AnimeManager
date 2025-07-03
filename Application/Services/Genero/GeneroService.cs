using Application.DTO.Genero;
using Infra.Repositories;

namespace Application.Services.Genero;

public class GeneroService : IGeneroService
{
    private readonly UnityOfWork _unityOfWork;

    public GeneroService(UnityOfWork unityOfWork)
    {
        _unityOfWork = unityOfWork;
    }

    public async Task<IEnumerable<GeneroDTO>> GetAllAsync()
    {
        var generos = await _unityOfWork.Generos.GetAll();
        return generos.Select(g => new GeneroDTO
        {
            Id = g.Id,
            Nome = g.Nome
        });
    }

    public async Task<GeneroDTO> GetByIdAsync(int id)
    {
        var genero = await _unityOfWork.Generos.GetById(id);
        if (genero == null) throw new Exception("Gênero não encontrado");

        return new GeneroDTO
        {
            Id = genero.Id,
            Nome = genero.Nome
        };
    }

    public async Task<GeneroDTO> CreateAsync(GeneroCreateDTO dto)
    {
        var genero = new Infra.Entities.Genero
        {
            Nome = dto.Nome
        };

        await _unityOfWork.Generos.Add(genero);
        await _unityOfWork.SaveChangesAsync();

        return new GeneroDTO
        {
            Id = genero.Id,
            Nome = genero.Nome
        };
    }

    public async Task UpdateAsync(int id, GeneroUpdateDTO dto)
    {
        var genero = await _unityOfWork.Generos.GetById(id);
        if (genero == null) throw new Exception("Gênero não encontrado");

        genero.Nome = dto.Nome;
        _unityOfWork.Generos.Update(genero);
        await _unityOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var genero = await _unityOfWork.Generos.GetById(id);
        if (genero == null) throw new Exception("Gênero não encontrado");

        _unityOfWork.Generos.Delete(genero);
        await _unityOfWork.SaveChangesAsync();
    }
}