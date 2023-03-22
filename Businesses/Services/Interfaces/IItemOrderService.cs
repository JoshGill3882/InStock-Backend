using instock_server_application.Businesses.Dtos;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace instock_server_application.Businesses.Services.Interfaces;

public interface IItemOrderService {
    Task<ItemOrderDto> CreateItemOrder(CreateItemOrderRequestDto createItemOrderRequestDto);
}