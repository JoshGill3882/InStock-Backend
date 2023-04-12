using instock_server_application.Businesses.Dtos;
using instock_server_application.Util.Dto;

namespace instock_server_application.Businesses.Services.Interfaces;

public interface IItemService {
    public Task<List<Dictionary<string, string>>?> GetItems(UserDto userDto, string businessId);
    Task<ItemDto> CreateItem(CreateItemRequestDto newItemRequestDto);
    public Task<StockUpdateDto> CreateStockUpdate(CreateStockUpdateRequestDto createStockUpdateRequestDto);

    Task<DeleteItemDto> DeleteItem(DeleteItemDto deleteItemDto);
    
    public Task<List<Dictionary<string, string>>?> GetCategories(ValidateBusinessIdDto validateBusinessIdDto);
}