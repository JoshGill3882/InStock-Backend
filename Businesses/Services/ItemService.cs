using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class ItemService : IItemService {
    private readonly IAmazonDynamoDB _client;

    public ItemService(IAmazonDynamoDB client) {
        _client = client;
    }

    public async Task<List<Item>?> GetItems(string businessId) {
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

        List<Item> items = new();

        foreach (Dictionary<string, AttributeValue> item in responseItems) {
            items.Add(
                new (
                    item["SKU"].S, 
                    item["BusinessId"].S,
                    item["Category"].S,
                    item["Name"].S,
                    Int32.Parse(item["Stock"].N)
                )
            );
        }

        return items;
    }
}