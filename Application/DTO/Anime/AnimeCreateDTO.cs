namespace Application.DTO.Anime;

public class AnimeCreateDTO
{
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;

    public static AnimeCreateDTO ToDTO(Infra.Entities.Anime anime)
    {
        return new AnimeCreateDTO()
        {
            Titulo = anime.Titulo,
            Descricao = anime.Descricao,
        };
    }
    public static Infra.Entities.Anime ToEntity(AnimeCreateDTO animeCreateDTO)
    {
        return new Infra.Entities.Anime()
        {
            Titulo = animeCreateDTO.Titulo,
            Descricao = animeCreateDTO.Descricao,
        };
    }
}