namespace Infra.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}