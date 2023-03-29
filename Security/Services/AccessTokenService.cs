using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using instock_server_application.Security.Models;
using instock_server_application.Security.Services.Interfaces;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace instock_server_application.Security.Services; 

public class AccessTokenService : IAccessTokenService {
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly SymmetricSecurityKey _jwtKey;
    private readonly IUserRepo _userRepo;
    
    public AccessTokenService(IOptions<JwtKey> jwtConfig, IUserRepo userRepo) {
        _jwtAudience = jwtConfig.Value.Audience;
        _jwtIssuer = jwtConfig.Value.Issuer;
        _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.SecretKey));
        _userRepo = userRepo;
    }

    /// <summary>
    /// Method which will be ran on a successful authentication
    /// Creates a JWT token which is then passed back
    /// </summary>
    /// <param name="id"> User's id </param>
    /// <param name="email"> User's email </param>
    /// <param name="businessId"> User's business id </param>
    /// <returns> JWT Token </returns>
    public string CreateToken(string id, string email, string businessId) {
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim("Id", id),
                new Claim("Email", email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("BusinessId", businessId)
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

    public bool CheckToken(DateTime dateTimeToCheck) {
        return dateTimeToCheck < DateTime.UtcNow;
    }

    public bool CheckBusiness(string? userBusinessId, string businessIdToCheck) {
        return !string.IsNullOrEmpty(userBusinessId) & businessIdToCheck.Equals(userBusinessId);
    }

    public string RefreshToAccess(string refreshToken) {
        User user = _userRepo.GetByRefreshToken(refreshToken).Result;

        if (string.IsNullOrEmpty(user.RefreshTokenExpiry) | !CheckToken(DateTime.Parse(user.RefreshTokenExpiry))) {
            return "Refresh Token Invalid";
        }

        return CreateToken(user.UserId, user.Email, user.BusinessId);
    }
}