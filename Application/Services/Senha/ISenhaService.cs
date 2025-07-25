namespace Application.Services.Senha;

public interface ISenhaService
{
    public void CriarHashSenha(string senha, out byte[] hash, out byte[] salt);
    public bool VerificaSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt);
}