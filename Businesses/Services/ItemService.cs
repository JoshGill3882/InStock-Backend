using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Models.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class ItemService : IItemService {
    private readonly IItemRepo _itemRepo;

    public ItemService(IItemRepo itemRepo) {
        _itemRepo = itemRepo;
    }

    public async Task<List<Item>?> GetItems(string businessId) {
        List<Dictionary<string, AttributeValue>> responseItems = _itemRepo.GetAllItems(businessId).Result;

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