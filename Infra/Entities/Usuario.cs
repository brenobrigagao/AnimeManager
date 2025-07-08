using System.ComponentModel.DataAnnotations;

namespace Infra.Entities;

public class Usuario : BaseEntity
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] SenhaHash { get; set; }
    public byte[] SenhaSalt { get; set; }
    public DateTime TokenDataCriacao { get; set; } = DateTime.Now;
    public bool IsAdmin { get; set; }
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    
}