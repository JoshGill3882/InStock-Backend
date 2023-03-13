using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IItemRepo {
    public Task<List<Dictionary<string, AttributeValue>>> GetAllItems(string businessId);
    Task<ItemDto> SaveNewItem(StoreItemDto itemToSaveDto);
    Task<bool> IsNameInUse(string businessId, string itemName);
    Task<bool> IsSKUInUse(string SKU, string businessId);
    public Task<ItemDto> GetItemWithUpdate(string itemId, JsonPatchDocument patchDocument);

    public Task<ItemDto> SaveExistingItem(StoreItemDto itemToSaveDto);

}