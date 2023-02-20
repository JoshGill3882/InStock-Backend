using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using instock_server_application.Users.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace instock_server_application.Users.Services; 

public class LoginService {
    private readonly IConfiguration? _config;
    private readonly IDynamoDBContext _context;

    public LoginService(IConfiguration? config, IDynamoDBContext context) {
        _config = config;
        _context = context;
    }

    /// <summary>
    /// Method which will be ran on a successful authentication
    /// Creates a JWT token which is then passed back
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <returns> JWT Token </returns>
    public string CreateToken(string email) {
        var issuer = _config.GetValue<string>("JWT:ISSUER");
        var audience = _config.GetValue<string>("JWT:AUDIENCE");
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Function for getting a User's data, given an email
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <returns> Returns User's Data, or "null" if the User is not found </returns>
    public async Task<User?> FindUserByEmail(string email) {
        var user = await _context.LoadAsync<User>(email);
        if (user == null) return null;
        return user;
    }

    public async Task<List<User>> GetAllUsers() {
        Console.Write("IN SERVICE");
        var users = await _context.ScanAsync<User>(default).GetRemainingAsync();
        Console.Write(users);
        return users;
    }
}