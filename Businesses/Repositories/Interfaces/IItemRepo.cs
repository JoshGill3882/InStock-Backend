using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IItemRepo {
    Task<List<Dictionary<string, AttributeValue>>> GetAllItems(string businessId);
    Task<ItemDto> SaveNewItem(StoreItemDto itemToSaveDto);
    Task<bool> IsNameInUse(CreateItemRequestDto createItemRequestDto);
    Task<bool> IsSKUInUse(string SKU, string businessId);
    void Delete(DeleteItemDto deleteItemDto);

}