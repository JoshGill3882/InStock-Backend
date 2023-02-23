using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace instock_server_application.Users.Services; 

public class LoginService : ILoginService {
    private readonly WebApplicationBuilder _builder = WebApplication.CreateBuilder();
    private readonly IAmazonDynamoDB _client;
    
    public LoginService(IAmazonDynamoDB client) {
        _client = client;
    }
    
    /// <summary>
    /// Method which will be ran on a successful authentication
    /// Creates a JWT token which is then passed back
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <returns> JWT Token </returns>
    public string CreateToken(string email) {
        string issuer = _builder.Configuration["JWT:ISSUER"]!;
        string audience = _builder.Configuration["JWT:AUDIENCE"]!;
        byte[] key = Encoding.ASCII.GetBytes(_builder.Configuration["JWT_KEY"]!);

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
        var request = new GetItemRequest {
            Key = new Dictionary<string, AttributeValue> { ["Email"] = new (email) },
            TableName = "Users"
        };
        var response = await _client.GetItemAsync(request);
        var result = response.Item;
        
        // If the Email does not contain the key "Email", return null
        // Means the given email was not found in the Database
        if (!result.ContainsKey("Email")) {
            return null;
        }
        
        var userDetails = new User(
            result["Email"].S,
            result["AccountStatus"].S,
            int.Parse(result["CreationDate"].N),
            result["FirstName"].S,
            result["LastName"].S,
            result["Password"].S,
            result["Role"].S
        );
        return userDetails;
    }
}