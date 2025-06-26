namespace Application.DTO.Genero;

public class GeneroCreateDTO
{
    public string Nome { get; set; } = null!;

    public static GeneroCreateDTO ToDTO(Infra.Entities.Genero genero)
    {
        return new GeneroCreateDTO()
        {
            Nome = genero.Nome,
        };   
    }

    public static Infra.Entities.Genero ToEntity(GeneroCreateDTO generoDTO)
    {
        return new Infra.Entities.Genero()
        {
            Nome = generoDTO.Nome,
        };  
    }
}