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
    public byte[] SenhaHash { get; set; }
    public byte[] SenhaSalt { get; set; }
    public DateTime TokenDataCriacao { get; set; } = DateTime.Now;
    public bool IsAdmin { get; set; }
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    
}