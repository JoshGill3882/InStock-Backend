using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Repositories.Interfaces;

public interface IItemRepo {
    Task<List<Dictionary<string, AttributeValue>>> GetAllItems(string businessId);
    Task<ItemDto> SaveNewItem(StoreItemDto itemToSaveDto);
    Task<bool> IsNameInUse(CreateItemRequestDto createItemRequestDto);
    Task<bool> IsSKUInUse(string SKU, string businessId);
    Task<ItemDto> SaveExistingItem(StoreItemDto itemToSaveDto);
    Task<StockUpdateDto> SaveStockUpdate(StoreStockUpdateDto stockUpdateDto);
    Task<ItemDto?> GetItem(string businessId, string itemSku);
    void Delete(DeleteItemDto deleteItemDto);
    Task<List<Dictionary<string, AttributeValue>>> GetAllCategories(ValidateBusinessIdDto validateBusinessIdDto);
}