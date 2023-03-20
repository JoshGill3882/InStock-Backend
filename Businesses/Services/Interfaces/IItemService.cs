using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace instock_server_application.Businesses.Services.Interfaces;

public interface IItemService {
    public Task<List<Dictionary<string, string>>?> GetItems(UserDto userDto, string businessId);
    Task<ItemDto> CreateItem(CreateItemRequestDto newItemRequestDto);
    Task<ItemDto> UpdateItem(UpdateItemRequestDto updateItemRequestDto);
    public Task<StockUpdateDto> CreateStockUpdate(CreateStockUpdateRequestDto createStockUpdateRequestDto);

}