namespace Application.DTO.Usuario;

public class UsuarioCreateDTO
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;

    public static UsuarioCreateDTO ToDTO(Infra.Entities.Usuario usuario)
    {
        return new UsuarioCreateDTO()
        {
            Nome = usuario.Nome,
            Email = usuario.Email,
            Senha = usuario.Senha,
        };   
    }

    public static Infra.Entities.Usuario ToEntity(UsuarioCreateDTO usuarioDTO)
    {
        return new Infra.Entities.Usuario()
        {
            Nome = usuarioDTO.Nome,
            Email = usuarioDTO.Email,
            Senha = usuarioDTO.Senha,
        };   
    }
}