using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IItemRepo {
    public Task<List<Dictionary<string, AttributeValue>>> GetAllItems(string businessId);
    Task<ItemDto> SaveNewItem(StoreItemDto itemToSaveDto);

}