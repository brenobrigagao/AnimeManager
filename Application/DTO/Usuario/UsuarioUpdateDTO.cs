using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Usuario;

public class UsuarioUpdateDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string NovaSenha { get; set; } = null!;

    public static UsuarioUpdateDTO ToDTO(Infra.Entities.Usuario usuario)
    {
        return new UsuarioUpdateDTO()
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            NovaSenha = usuario.Senha,
        };   
    }
    public static Infra.Entities.Usuario ToEntity(UsuarioUpdateDTO usuarioDTO)
    {
        return new Infra.Entities.Usuario()
        {
            Nome = usuarioDTO.Nome,
            Email = usuarioDTO.Email,
            Senha = usuarioDTO.NovaSenha,
        };
    }
}