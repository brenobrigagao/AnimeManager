namespace Infra.Entities;

public class PasswordResetToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool Usado { get; set; }
    public int usuarioId { get; set; }
    public Usuario Usuario { get; set; }
}