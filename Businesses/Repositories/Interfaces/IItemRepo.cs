using Amazon.DynamoDBv2.Model;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IItemRepo {
    public Task<List<Dictionary<string, AttributeValue>>> GetAllItems(string businessId);
}