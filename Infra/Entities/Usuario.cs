using System.ComponentModel.DataAnnotations;

namespace Infra.Entities;

public class Usuario : BaseEntity
{
    [Required]
    [MaxLength(30)]
    public string Nome { get; set; } = null!;

    [Required] 
    [EmailAddress] 
    public string Email { get; set; } = null!;
    [Required] 
    public string Senha { get; set; } = null!;
    public bool IsAdmin { get; set; }
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();

    
}