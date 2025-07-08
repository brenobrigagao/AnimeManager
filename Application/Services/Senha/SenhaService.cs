using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Senha;

public class SenhaService : ISenhaService
{
    private readonly IConfiguration _config;
    public SenhaService(IConfiguration config)
    {
        _config = config;
    }
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

    public string CriarToken(Infra.Entities.Usuario usuario)
    {
        List<Claim> Claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role,usuario.IsAdmin ? "Admin" : "User"),
            new Claim("IsAdmin", usuario.IsAdmin.ToString())
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
        var cred =  new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: Claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: cred
        );
        var jwt = new  JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public string GerarRefreshToken()
    {
        var randomBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
    
}