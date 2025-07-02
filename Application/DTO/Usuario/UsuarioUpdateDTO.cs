using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Usuario;

public class UsuarioUpdateDTO
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(30)]
    public string Nome { get; set; } = null!;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [MinLength(6, ErrorMessage = "A nova senha deve ter no mínimo 6 caracteres")]
    public string NovaSenha { get; set; } = null!;

    // Método para transformar ENTIDADE em DTO
    public static UsuarioUpdateDTO ToDTO(Infra.Entities.Usuario usuario)
    {
        return new UsuarioUpdateDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            NovaSenha = string.Empty // nunca expõe a senha
        };
    }

    // Método para aplicar atualização à entidade existente
    public void ApplyToEntity(Infra.Entities.Usuario usuario, byte[] novaHash, byte[] novoSalt)
    {
        usuario.Nome = Nome;
        usuario.Email = Email;
        usuario.SenhaHash = novaHash;
        usuario.SenhaSalt = novoSalt;
    }
}