namespace Application.DTO.Anime;

public class AnimeCreateDTO
{
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public int EstudioId { get; set; }
    public int GeneroId { get; set; }
    public int Ano { get; set; }

    public static AnimeCreateDTO ToDTO(Infra.Entities.Anime anime)
    {
        return new AnimeCreateDTO()
        {
            Titulo = anime.Titulo,
            Descricao = anime.Descricao,
            EstudioId = anime.EstudioId,
            GeneroId = anime.GeneroId,
            Ano = anime.Ano
        };
    }
    public static Infra.Entities.Anime ToEntity(AnimeCreateDTO animeCreateDTO)
    {
        return new Infra.Entities.Anime()
        {
            Titulo = animeCreateDTO.Titulo,
            Descricao = animeCreateDTO.Descricao,
            EstudioId = animeCreateDTO.EstudioId,
            GeneroId = animeCreateDTO.GeneroId,
            Ano = animeCreateDTO.Ano
        };
    }
}