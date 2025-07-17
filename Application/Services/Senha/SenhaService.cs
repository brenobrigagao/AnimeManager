using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Senha;

public class SenhaService : ISenhaService
{
    public void CriarHashSenha(string senha, out byte[] hash, out byte[] salt)
    {
        using (var hmac = new HMACSHA512()){
        salt = hmac.Key;
        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
        }
    }

    public bool VerificaSenhaHash(string senha,byte[] senhaHash, byte[] senhaSalt)
    {
        using (var hmac = new HMACSHA512(senhaSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            return computedHash.SequenceEqual(senhaHash);
        }
        
    }
    
}