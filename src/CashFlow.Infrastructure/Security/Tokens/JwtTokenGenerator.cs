using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace CashFlow.Infrastructure.Security.Tokens;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationTimeInMinutes;
    private readonly string _signinKey;

    public JwtTokenGenerator(uint expirationTimeInMinutes, string signinKey)
    {
        _expirationTimeInMinutes = expirationTimeInMinutes;
        _signinKey = signinKey;
    }

    public string Generate(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
        };
        
        var securityKey = SecurityKey();
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signinKey);
        
        return new SymmetricSecurityKey(key);
    }
}