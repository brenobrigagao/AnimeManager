namespace Infra.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public bool Revogado { get; set; } = false;
    
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

}