using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Usuario;

public class UsuarioCreateDTO
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(30, ErrorMessage = "O nome pode ter no máximo 30 caracteres")]
    public string Nome { get; set; } = null!;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "O email fornecido não é válido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string Senha { get; set; } = null!;

    [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
    public string ConfirmarSenha { get; set; } = null!;

    public Infra.Entities.Usuario ToEntity(byte[] senhaHash, byte[] senhaSalt)
    {
        return new Infra.Entities.Usuario
        {
            Nome = this.Nome,
            Email = this.Email,
            SenhaHash = senhaHash,
            SenhaSalt = senhaSalt,
            TokenDataCriacao = DateTime.UtcNow,
            IsAdmin = false
        };
    }
}