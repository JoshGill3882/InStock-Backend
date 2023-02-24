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
    private readonly IAmazonDynamoDB _client;
    
    public LoginService(IAmazonDynamoDB client) {
        _client = client;
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