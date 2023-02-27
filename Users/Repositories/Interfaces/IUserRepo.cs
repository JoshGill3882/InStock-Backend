using Amazon.DynamoDBv2.Model;

namespace instock_server_application.Users.Repositories.Interfaces; 

public interface IUserRepo {
    public Task<Dictionary<string, AttributeValue>?> GetUser(string email);
}