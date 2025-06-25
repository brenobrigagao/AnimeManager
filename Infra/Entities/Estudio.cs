using System.ComponentModel.DataAnnotations;

namespace Infra.Entities;

public class Estudio : BaseEntity
{ 
    [Required]
    [MaxLength(20)]
    public string Nome { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string Descricao { get; set; } = null!;
    public ICollection<Anime> Animes { get; set; } = new List<Anime>();
}