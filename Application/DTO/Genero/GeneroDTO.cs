namespace Application.DTO.Genero;

public class GeneroDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;

    public static GeneroDTO ToDTO(Infra.Entities.Genero genero)
    {
        return new GeneroDTO()
        {
            Id = genero.Id,
            Nome = genero.Nome,
        };
    }
    public static Infra.Entities.Genero ToEntity(GeneroDTO generoDTO)
    {
        return new Infra.Entities.Genero()
        {
            Nome = generoDTO.Nome,
        };
    }   
}