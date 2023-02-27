using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;

namespace instock_server_application.Users.Repositories; 

public class UserRepo : IUserRepo{
    private readonly IAmazonDynamoDB _client;
    
    public UserRepo(IAmazonDynamoDB client) {
        _client = client;
    }
    
    /// <summary>
    /// Method for getting the data related to a user, returning null if nothing is found
    /// </summary>
    /// <param name="email"> The Email to search by </param>
    /// <returns> The User's Details, or Null </returns>
    public async Task<Dictionary<string, AttributeValue>?> GetUser(string email) {
        var request = new QueryRequest {
            TableName = User.TableName,
            IndexName = "Email",
            KeyConditionExpression = "Email = :Email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":Email", new AttributeValue(email)}
            }
        };
        var response = await _client.QueryAsync(request);
        return response.Items[0];
    }
}