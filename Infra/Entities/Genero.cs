namespace Infra.Entities;

public class Genero
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public ICollection<Anime> Animes { get; set; } = new List<Anime>();
}