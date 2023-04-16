using instock_server_application.Businesses.Dtos;
using instock_server_application.Util.Dto;

namespace instock_server_application.Businesses.Services.Interfaces;

public interface IItemService {
    Task<ListOfItemDto> GetItems(UserDto userDto, string businessId);
    Task<ItemDetailsDto> GetItem(UserAuthorisationDto userAuthorisationDto, ItemRequestDto itemRequestDto);
    Task<ItemDto> CreateItem(CreateItemRequestDto newItemRequestDto);
    public Task<StockUpdateDto> CreateStockUpdate(CreateStockUpdateRequestDto createStockUpdateRequestDto);
    Task<DeleteItemDto> DeleteItem(DeleteItemDto deleteItemDto);
    Task<List<Dictionary<string, string>>?> GetCategories(ValidateBusinessIdDto validateBusinessIdDto);
}