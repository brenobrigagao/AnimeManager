using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Senha;

public class SenhaService : ISenhaService
{
    
    public void CriarHashSenha(string senha, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
    }
}