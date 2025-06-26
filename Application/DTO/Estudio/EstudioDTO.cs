namespace Application.DTO.Estudio;

public class EstudioDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    
    public static EstudioDTO ToDTO(Infra.Entities.Estudio estudio)
    {
        return new EstudioDTO()
        {
            Id = estudio.Id,
            Nome = estudio.Nome,
            Descricao = estudio.Descricao,
        };
    }
    public static Infra.Entities.Estudio ToEntity(EstudioDTO estudioDTO)
    {
        return new Infra.Entities.Estudio()
        {
            Nome = estudioDTO.Nome,
            Descricao = estudioDTO.Descricao,
        };
    }
}