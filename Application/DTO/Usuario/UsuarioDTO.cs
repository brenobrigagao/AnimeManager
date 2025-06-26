namespace Application.DTO.Usuario;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;

    public static UsuarioDTO ToDTO(Infra.Entities.Usuario usuario)
    {
        return new UsuarioDTO()
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
        };
    }

    public static Infra.Entities.Usuario ToEntity(UsuarioDTO usuarioDTO)
    {
        return new Infra.Entities.Usuario()
        {
            Nome = usuarioDTO.Nome,
            Email = usuarioDTO.Email,
        };
    }
}