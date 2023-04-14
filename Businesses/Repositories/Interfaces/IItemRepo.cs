using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Repositories.Interfaces;

public interface IItemRepo {
    Task<List<ItemDto>> GetAllItems(string businessId);
    Task<List<StatItemDto>> GetAllItemsStatsDetails(string businessId);
    Task<ItemDto> SaveNewItem(StoreItemDto itemToSaveDto);
    Task<bool> IsNameInUse(CreateItemRequestDto createItemRequestDto);
    Task<bool> IsSkuInUse(string sku);
    Task<ItemDto> SaveExistingItem(StoreItemDto itemToSaveDto);
    Task<StockUpdateDto> SaveStockUpdate(StoreStockUpdateDto stockUpdateDto);
    Task<ItemDto?> GetItem(string businessId, string itemSku);
    void Delete(DeleteItemDto deleteItemDto);
    Task<List<Dictionary<string, string>>> GetAllCategories(ValidateBusinessIdDto validateBusinessIdDto);
    Task<ItemOrderDto> SaveItemOrder(StoreItemOrderDto storeItemOrderDto);

}