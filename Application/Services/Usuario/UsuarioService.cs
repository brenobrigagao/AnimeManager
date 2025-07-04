using Application.DTO.Usuario;
using Infra.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Application.Services.Senha;

namespace Application.Services.Usuario;

public class UsuarioService : IUsuarioService
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly ISenhaService _senhaService;

    public UsuarioService(IUnityOfWork unityOfWork,ISenhaService senhaService)
    {
        _unityOfWork = unityOfWork;
        _senhaService = senhaService;
    }

    public async Task<UsuarioDTO> GetByIdAsync(int id)
    {
        var usuario = await _unityOfWork.Usuarios.GetById(id);
        if (usuario == null) throw new KeyNotFoundException("Usuário não encontrado");

        return new UsuarioDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }

    public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
    {
        var usuarios = await _unityOfWork.Usuarios.GetAll();
        return usuarios.Select(u => new UsuarioDTO
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email
        });
    }

    public async Task<UsuarioDTO> CreateAsync(UsuarioCreateDTO dto)
    {
        var existente = await _unityOfWork.Usuarios.GetByEmail(dto.Email);
        if (existente != null)
            throw new Exception("Esse email já está cadastrado");

        _senhaService.CriarHashSenha(dto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

        var usuario = new Infra.Entities.Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = senhaHash,
            SenhaSalt = senhaSalt,
            TokenDataCriacao = DateTime.UtcNow,
            IsAdmin = false
        };

        await _unityOfWork.Usuarios.Add(usuario);
        await _unityOfWork.SaveChangesAsync();

        return new UsuarioDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }

    public async Task UpdateAsync(int id, UsuarioUpdateDTO dto)
    {
        var usuario = await _unityOfWork.Usuarios.GetById(id);
        if (usuario == null)
            throw new KeyNotFoundException("Usuário não encontrado");

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.NovaSenha))
        {
            _senhaService.CriarHashSenha(dto.NovaSenha, out byte[] novaHash, out byte[] novoSalt);
            usuario.SenhaHash = novaHash;
            usuario.SenhaSalt = novoSalt;
        }

        _unityOfWork.Usuarios.Update(usuario);
        await _unityOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await _unityOfWork.Usuarios.GetById(id);
        if (usuario == null)
            throw new KeyNotFoundException("O usuário não foi encontrado");

        _unityOfWork.Usuarios.Delete(usuario);
        await _unityOfWork.SaveChangesAsync();
    }

    public async Task<UsuarioDTO> CreateAdminAsync(UsuarioCreateDTO dto)
    {
        var existe = await _unityOfWork.Usuarios.GetByEmail(dto.Email);
        if (existe != null) throw new Exception("Esse email já está cadastrado");
        
        _senhaService.CriarHashSenha(dto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

        var usuario = new Infra.Entities.Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = senhaHash,
            SenhaSalt = senhaSalt,
            TokenDataCriacao = DateTime.UtcNow,
            IsAdmin = true
        };
        
        await _unityOfWork.Usuarios.Add(usuario);
        await _unityOfWork.SaveChangesAsync();
        return new UsuarioDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }

    public async Task UpdateAdminAsync(int id)
    {
        var usuario = await _unityOfWork.Usuarios.GetById(id);
        if (usuario == null) throw new KeyNotFoundException("Usuário não encontrado!");

        usuario.IsAdmin = true;
        _unityOfWork.Usuarios.Update(usuario);
        await  _unityOfWork.SaveChangesAsync();
        
    }

    
}
