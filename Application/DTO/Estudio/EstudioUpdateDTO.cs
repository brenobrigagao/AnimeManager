namespace Application.DTO.Estudio;

public class EstudioUpdateDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;

    public static EstudioUpdateDTO ToDTO(Infra.Entities.Estudio estudio)
    {
        return new EstudioUpdateDTO()
        {
            Id = estudio.Id,
            Nome = estudio.Nome,
            Descricao = estudio.Descricao,
        };
    }

    public static Infra.Entities.Estudio ToEntity(EstudioUpdateDTO estudioUpdateDTO)
    {
        return new Infra.Entities.Estudio()
        {
            Nome = estudioUpdateDTO.Nome,
            Descricao = estudioUpdateDTO.Descricao,
        };  
    }
}