using System.ComponentModel.DataAnnotations;

namespace Infra.Entities;

public class Avaliacao : BaseEntity
{
    [Range(0,10)]
    public double Nota { get; set; }
    [MaxLength(1000)]
    public string Comentario { get; set; } = null!;
    [Required]
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    [Required]
    public int AnimeId { get; set; }
    public Anime? Anime { get; set; }
}