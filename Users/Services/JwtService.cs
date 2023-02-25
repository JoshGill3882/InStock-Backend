using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using instock_server_application.Users.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace instock_server_application.Users.Services; 

public class JwtService : IJwtService {
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly SymmetricSecurityKey _jwtKey;
    
    public JwtService(IOptions<JwtKey> jwtConfig) {
        _jwtAudience = jwtConfig.Value.Audience;
        _jwtIssuer = jwtConfig.Value.Issuer;
        _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.SecretKey));
    }

    /// <summary>
    /// Method which will be ran on a successful authentication
    /// Creates a JWT token which is then passed back
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <returns> JWT Token </returns>
    public string CreateToken(User user) {

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim("Id", user.UserId),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("BusinessIds", user.Businesses.ToString()!)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _jwtIssuer,
            Audience = _jwtAudience,
            SigningCredentials = new SigningCredentials(
                _jwtKey,
                SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}