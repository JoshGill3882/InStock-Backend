using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services.Interfaces;

namespace instock_server_application.Users.Services; 

public class UserService : IUserService {
    private readonly IAmazonDynamoDB _client;
    
    public UserService(IAmazonDynamoDB client) {
        _client = client;
    }
    
    /// <summary>
    /// Function for getting a User's data, given an email
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <returns> Returns User's Data, or "null" if the User is not found </returns>
    public async Task<User?> FindUserByEmail(string email) {
        var request = new QueryRequest {
            TableName = "Users",
            IndexName = "Email",
            KeyConditionExpression = "Email = :Email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":Email", new AttributeValue(email)}
            }
        };
        var response = await _client.QueryAsync(request);

        // If there was no email found using the QueryRequest then there is no user using that email
        if (response.Count <= 0) {
            return null;
        }
        
        var result = response.Items[0];
        
        // If the Email does not contain the key "Email", return null
        // Means the given email was not found in the Database
        if (!result.ContainsKey("Email")) {
            return null;
        }

        // Setting the business Id depending if it's null or not
        // TODO Changes this when refactoring multiple businessIds to single businessId
        string businessId = "";
        if (result.ContainsKey("Businesses")) {
            businessId = result["Businesses"].L[0].S;
        }
        if (result.ContainsKey("BusinessId")) {
            businessId = result["BusinessId"].S;
        }
        
        var userDetails = new User(
            result["UserId"].S,
            result["Email"].S,
            result["AccountStatus"].S,
            int.Parse(result["CreationDate"].N),
            result["FirstName"].S,
            result["LastName"].S,
            result["Password"].S,
            result["Role"].S,
            businessId,
            result["RefreshToken"].S
        );
        return userDetails;
    }
}