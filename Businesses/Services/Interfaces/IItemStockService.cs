using instock_server_application.Businesses.Dtos;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace instock_server_application.Businesses.Services.Interfaces;

public interface IItemStockService {
    Task<StockUpdateDto> CreateStockUpdate(CreateStockUpdateRequestDto createStockUpdateRequestDto);
}