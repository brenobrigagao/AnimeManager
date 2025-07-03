using Application.DTO.Estudio;
using Infra.Repositories;

namespace Application.Services.Estudio;

public class EstudioService : IEstudioService
{
    private readonly UnityOfWork _unityOfWork;

    public EstudioService(UnityOfWork unityOfWork)
    {
        _unityOfWork = unityOfWork;
    }

    public async Task<IEnumerable<EstudioDTO>> GetAllAsync()
    {
        var estudios = await _unityOfWork.Estudios.GetAll();
        return estudios.Select(e => new EstudioDTO
        {
            Id = e.Id,
            Nome = e.Nome,
            Descricao = e.Descricao
        });
    }

    public async Task<EstudioDTO> GetByIdAsync(int id)
    {
        var estudio = await _unityOfWork.Estudios.GetById(id);
        if (estudio == null) throw new Exception("Estúdio não encontrado");

        return new EstudioDTO
        {
            Id = estudio.Id,
            Nome = estudio.Nome,
            Descricao = estudio.Descricao
        };
    }

    public async Task<EstudioDTO> CreateAsync(EstudioCreateDTO dto)
    {
        var estudio = new Infra.Entities.Estudio
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao
        };

        await _unityOfWork.Estudios.Add(estudio);
        await _unityOfWork.SaveChangesAsync();

        return new EstudioDTO
        {
            Id = estudio.Id,
            Nome = estudio.Nome,
            Descricao = estudio.Descricao
        };
    }

    public async Task UpdateAsync(int id, EstudioUpdateDTO dto)
    {
        var estudio = await _unityOfWork.Estudios.GetById(id);
        if (estudio == null) throw new Exception("Estúdio não encontrado");

        estudio.Nome = dto.Nome;
        estudio.Descricao = dto.Descricao;

        _unityOfWork.Estudios.Update(estudio);
        await _unityOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var estudio = await _unityOfWork.Estudios.GetById(id);
        if (estudio == null) throw new Exception("Estúdio não encontrado");

        _unityOfWork.Estudios.Delete(estudio);
        await _unityOfWork.SaveChangesAsync();
    }
}
