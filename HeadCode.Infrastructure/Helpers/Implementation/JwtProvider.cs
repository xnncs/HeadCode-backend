namespace HeadCode.Infrastructure.Helpers.Implementation;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Abstract;
using Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new(nameof(user.Id), user.Id.ToString())
        ];

        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        SymmetricSecurityKey key = new(secretKeyBytes);

        SigningCredentials singingCredentials = new(key, SecurityAlgorithms.HmacSha256);

        DateTime timeExpires = DateTime.UtcNow.AddHours(_options.ExpiresHours);

        JwtSecurityToken token = new(
            claims: claims,
            signingCredentials: singingCredentials,
            expires: timeExpires
        );

        JwtSecurityTokenHandler handler = new();

        string tokenValue = handler.WriteToken(token);
        return tokenValue;
    }

    public string GetIdFromClaims(string jwtToken)
    {
        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);

        JwtSecurityTokenHandler handler = new();

        TokenValidationParameters validations = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        ClaimsPrincipal claims = handler.ValidateToken(jwtToken, validations, out SecurityToken? tokenSecure);

        return claims.FindFirst(x => x.Type == nameof(User.Id))!.Value;
    }
}