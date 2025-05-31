﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using onlineshop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DomainService.Services;

public class TokenService(IOptions<Settings> options) : ITokenService
{
    private readonly JWTSettings _settings = options.Value.JWT;

    public string Generate(string username, List<string> permissions)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, username)
        };

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permissions", permission));
        }

        var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_settings.ExpireInMinutes),
                signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
