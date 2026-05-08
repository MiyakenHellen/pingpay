using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PingPay.Identity.Domain.Entities;
using PingPay.Identity.Application.Abstractions;

namespace PingPay.Identity.Infrastructure.Security;

public class JwtTokenService(IOptions<JwtSettings> options) : ITokenService
{
    private readonly JwtSettings _settings = options.Value;

    public string GenerateToken(Merchant merchant)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        
        Claim[] claims = [
            new Claim(JwtRegisteredClaimNames.Sub, merchant.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, merchant.Email),
            new Claim("merchant_id", merchant.Id.ToString()),
            new Claim("api_key", merchant.ApiKey),
            new Claim(ClaimTypes.Role, "Merchant")
        ];
        
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}