namespace Application.DTO.Genero;

public class GeneroUpdateDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;

    public static GeneroUpdateDTO ToDTO(Infra.Entities.Genero genero)
    {
        return new GeneroUpdateDTO()
        {
            Id = genero.Id,
            Nome = genero.Nome,
        };
    }
    public static Infra.Entities.Genero ToEntity(GeneroUpdateDTO generoUpdateDTO)
    {
        return new Infra.Entities.Genero()
        {
            Nome = generoUpdateDTO.Nome,
        };
    }  
}