namespace Application.DTO.Avaliacao;

public class AvaliacaoCreateDTO
{
    public double Nota { get; set; }
    public string Comentario { get; set; } = null!;
    public int UsuarioId { get; set; }
    public int AnimeId { get; set; }

    public static AvaliacaoCreateDTO ToDTO(Infra.Entities.Avaliacao avaliacao)
    {
        return new AvaliacaoCreateDTO()
        {
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
        }; 
    }
    public static Infra.Entities.Avaliacao ToEntity(AvaliacaoCreateDTO avaliacaoCreateDTO)
    {
        return new Infra.Entities.Avaliacao()
        {
            Nota = avaliacaoCreateDTO.Nota,
            Comentario = avaliacaoCreateDTO.Comentario,
        }; 
    }  
}