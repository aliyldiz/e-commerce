using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ECommerceApi.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceApi.Infrastructure.Services.Token;

public class TokenHandler : ITokenHandler
{
    private readonly IConfiguration _configuration;

    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Application.DTOs.Token CreateAccessToken(int second)
    {
        Application.DTOs.Token token = new();
        
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]!));
        
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        
        token.Expiration = DateTime.UtcNow.AddMinutes(second);
        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: _configuration["Token:Issuer"],
            audience: _configuration["Token:Audience"],
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials
        );
        
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        
        token.AccessToken = tokenHandler.WriteToken(securityToken);
 
        return token;
    }
}