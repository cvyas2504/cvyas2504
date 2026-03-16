using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FrontOfficeERP.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace FrontOfficeERP.API.Auth;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user, string roleName)
    {
        var jwtConfig = configuration.GetSection("Jwt");
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(ClaimTypes.Role, roleName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfig["Issuer"],
            audience: jwtConfig["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
