using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Resumi.App.Data.Models;
using Resumi.Infra.Auth.Interfaces;

namespace Resumi.Infra.Auth;

public class AuthManager : IAuthManager
{
    private readonly JwtAuthSettings _jwtAuthSettings;

    public AuthManager(JwtAuthSettings jwtAuthSettings)
    {
        _jwtAuthSettings = jwtAuthSettings;
    }

    private const int ExpiryDurationMinutes = 60;

    public AuthResponse NewAuthResponse(AppUser user)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddMinutes(ExpiryDurationMinutes).ToString("o")),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!)
        ];

        SymmetricSecurityKey symetricKey = new(Encoding.UTF8.GetBytes(_jwtAuthSettings.Secret));

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(ExpiryDurationMinutes),
            Issuer = _jwtAuthSettings.Issuer,
            Audience = _jwtAuthSettings.Audience,
            SigningCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponse
        {
            Token = tokenHandler.WriteToken(token),
            ExpiresAt = tokenDescriptor.Expires.Value
        };
    }
}