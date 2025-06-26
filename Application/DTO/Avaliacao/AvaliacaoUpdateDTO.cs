namespace Application.DTO.Avaliacao;

public class AvaliacaoUpdateDTO
{
    public int Id { get; set; }
    public double Nota { get; set; }
    public string Comentario { get; set; } = null!;

    public static AvaliacaoUpdateDTO ToDTO(Infra.Entities.Avaliacao avaliacao)
    {
        return new AvaliacaoUpdateDTO()
        {
            Id = avaliacao.Id,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
        };
    }

    public static Infra.Entities.Avaliacao ToEntity(AvaliacaoUpdateDTO avaliacaoUpdateDTO)
    {
        return new Infra.Entities.Avaliacao()
        {
            Nota = avaliacaoUpdateDTO.Nota,
            Comentario = avaliacaoUpdateDTO.Comentario,
        }; 
    }
}