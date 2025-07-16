namespace Application.DTO.Anime;

public class AnimeDTO
{
    public int Id { get; set; }
    public string Descricao { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public double MediaNota { get; set; }

    public static AnimeDTO ToDTO(Infra.Entities.Anime anime)
    {
        return new AnimeDTO()
        {
            Id = anime.Id,
            Descricao = anime.Descricao,
            Titulo = anime.Titulo,
            MediaNota = anime.MediaNota
        };
    }
    public static Infra.Entities.Anime ToEntity(AnimeDTO animeDTO)
    {
        return new Infra.Entities.Anime()
        {
            Descricao = animeDTO.Descricao,
            Titulo = animeDTO.Titulo,
            MediaNota = animeDTO.MediaNota
        };
    }
}