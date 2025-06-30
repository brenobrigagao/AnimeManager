using Application.DTO.Usuario;
using Infra.Repositories;
using Infra.Repositories.Interfaces;

namespace Application.Services.Usuario;

public class UsuarioService : IUsuarioService
{
    private readonly IUnityOfWork _unitOfWork;
    public UsuarioService(IUnityOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UsuarioDTO> GetByIdAsync(int id)
    {
        var usuario = await _unitOfWork.Usuarios.GetById(id);
        if (usuario == null) throw new KeyNotFoundException("Usuario não encontrado");
       
        return new UsuarioDTO()
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }

    public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
    {
        var usuarios = await _unitOfWork.Usuarios.GetAll();
        return usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email
            }
        );
    }

    public async Task<UsuarioDTO> CreateAsync(UsuarioCreateDTO dto)
    {
        var existente = _unitOfWork.Usuarios.GetByEmail(dto.Email);
        if (existente != null)
        {
            throw new Exception("Esse email já está cadastrado");
        }
        var usuario = new Infra.Entities.Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            IsAdmin = false
        };

        await _unitOfWork.Usuarios.Add(usuario);
        await _unitOfWork.SaveChangesAsync(); 

        return new UsuarioDTO
        { 
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }

    public async Task UpdateAsync(int id, UsuarioUpdateDTO dto)
    {
        var usuario = await _unitOfWork.Usuarios.GetById(id);
        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuario não encontrado");
        }
        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        if (!string.IsNullOrWhiteSpace(dto.NovaSenha))
        {
            usuario.Senha = dto.NovaSenha;
        }
        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await _unitOfWork.Usuarios.GetById(id);
        if (usuario == null)
        {
            throw new KeyNotFoundException("O usuário vão foi encontrado");
        }
        _unitOfWork.Usuarios.Delete(usuario);
        _unitOfWork.SaveChangesAsync();
    }

}