namespace Application.DTO.Anime;

public class AnimeUpdateDTO
{
    public int Id { get; set; }
    public string Descricao { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    
    public static AnimeUpdateDTO ToDTO(Infra.Entities.Anime anime)
    {
        return new AnimeUpdateDTO()
        {
            Id = anime.Id,
            Descricao = anime.Descricao,
            Titulo = anime.Titulo,
        };
    }
    public static Infra.Entities.Anime ToEntity(AnimeUpdateDTO animeUpdateDTO)
    {
        return new Infra.Entities.Anime()
        {
            Descricao = animeUpdateDTO.Descricao,
            Titulo = animeUpdateDTO.Titulo,
        };
    }
}