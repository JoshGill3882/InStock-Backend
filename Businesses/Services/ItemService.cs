using System.Security.Claims;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Models.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class ItemService : IItemService {
    private readonly IItemRepo _itemRepo;
    private readonly IBusinessService _businessService;

    public ItemService(IItemRepo itemRepo, IBusinessService businessService) {
        _itemRepo = itemRepo;
        _businessService = businessService;
    }

    public async Task<List<Item>?> GetItems(ClaimsPrincipal user, string businessId) {
        if (_businessService.CheckBusinessIdInJWT(user, businessId)) {
            List<Dictionary<string, AttributeValue>> responseItems = _itemRepo.GetAllItems(businessId).Result;
            List<Item> items = new();

            // User has access, but incorrect businessID or no items found
            if (responseItems.Count == 0) {
                // Return an empty list
                return items;
            }

            foreach (Dictionary<string, AttributeValue> item in responseItems) {
                items.Add(
                    new(
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

        // If the user doesn't have access, return "null"
        return null;
    }
}