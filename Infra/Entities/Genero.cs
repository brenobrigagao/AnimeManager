using System.ComponentModel.DataAnnotations;

namespace Infra.Entities;

public class Genero : BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string Nome { get; set; } = null!;
    public ICollection<Anime> Animes { get; set; } = new List<Anime>();
}