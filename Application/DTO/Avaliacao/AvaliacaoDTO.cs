namespace Application.DTO.Avaliacao;

public class AvaliacaoDTO
{
    public int Id { get; set; }
    public double Nota { get; set; }
    public string Comentario { get; set; } = null!;

    public static AvaliacaoDTO ToDTO(Infra.Entities.Avaliacao avaliacao)
    {
        return new AvaliacaoDTO()
        {
            Id = avaliacao.Id,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
        };   
    }
    public static Infra.Entities.Avaliacao ToEntity(AvaliacaoDTO avaliacaoDTO)
    {
        return new Infra.Entities.Avaliacao()
        {
            Nota = avaliacaoDTO.Nota,
            Comentario = avaliacaoDTO.Comentario,
        };  
    }  
}