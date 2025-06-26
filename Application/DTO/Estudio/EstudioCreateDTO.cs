namespace Application.DTO.Estudio;

public class EstudioCreateDTO
{
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;

    public static EstudioCreateDTO ToDTO(Infra.Entities.Estudio estudio)
    {
        return new EstudioCreateDTO()
        {
            Nome = estudio.Nome,
            Descricao = estudio.Descricao,
        };
    }

    public static Infra.Entities.Estudio ToEntity(EstudioCreateDTO estudioCreateDTO)
    {
        return new Infra.Entities.Estudio()
        {
            Nome = estudioCreateDTO.Nome,
            Descricao = estudioCreateDTO.Descricao,
        }; 
    }
}