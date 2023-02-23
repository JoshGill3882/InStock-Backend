using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Items.Models;
using instock_server_application.Items.Services.Interfaces;

namespace instock_server_application.Items.Services; 

public class ItemService : IItemService {
    private readonly IAmazonDynamoDB _client;

    public ItemService(IAmazonDynamoDB client) {
        _client = client;
    }

    public async Task<List<Dictionary<string, string>>?> GetItems(string businessId) {
        var request = new QueryRequest {
            TableName = "Items",
            IndexName = "BusinessId",
            KeyConditionExpression = "BusinessId = :Id",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":Id", new AttributeValue(businessId)}
            }
        };
        var response = await _client.QueryAsync(request);
        var responseItems = response.Items;

        if (responseItems.Count == 0) {
            return null;
        }

        List<Dictionary<string, string>> items = new();

        foreach (Dictionary<string, AttributeValue> item in responseItems) {
            items.Add(
                new Dictionary<string, string> {
                    { "SKU", item["SKU"].S }, 
                    { "BusinessId", item["BusinessId"].S },
                    { "Category", item["Category"].S},
                    { "Name", item["Name"].S},
                    { "Stock", item["Stock"].N}
                }
            );
        }

        return items;
    }
}