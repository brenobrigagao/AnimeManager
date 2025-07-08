using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infra.Entities;

public class PasswordResetToken
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Token { get; set; }
    [Required]
    public DateTime Expires { get; set; }
    public bool Usado { get; set; }
    [Required]
    public int usuarioId { get; set; }
    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }
}