namespace Application.Services.Token;

public interface ITokenService
{
    public string CriarToken(Infra.Entities.Usuario usuario);
    public string GerarRefreshToken();
}