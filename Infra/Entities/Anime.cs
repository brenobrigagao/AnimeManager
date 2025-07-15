using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infra.Entities;

public class Anime : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Titulo { get; set; } = null!;
    [MaxLength(1000)]
    public string Descricao { get; set; } = null!;
    [Required]
    public int EstudioId { get; set; }
    public Estudio Estudio { get; set; }
    [Required]
    public int GeneroId { get; set; }
    public Genero Genero { get; set; }
    public int Ano { get; set; }
    public ICollection<Avaliacao> Avaliacoes { get; set; }
    public double MediaNota { get; set; }
}