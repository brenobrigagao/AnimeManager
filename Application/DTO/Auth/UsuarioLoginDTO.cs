using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Auth;

public class UsuarioLoginDTO
{
    [Required(ErrorMessage = "O campo email é obrigatorio"),EmailAddress(ErrorMessage = "Email inválido!")]
    public string Email { get; set; } 
    [Required(ErrorMessage = "O campo senha é obrigatorio")]
    public string Senha { get; set; }
}