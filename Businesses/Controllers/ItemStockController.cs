using System.Security.Claims;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Route("/businesses/{businessId}")]
public class ItemStockController : ControllerBase {
    private readonly IItemStockService _itemStockService;
    
    public ItemStockController(IItemStockService itemStockService) {
        _itemStockService = itemStockService;
    }
    
    [HttpPost]
    [Route("items/{itemSku}/stock/updates")]
    public async Task<IActionResult> CreateStockUpdate([FromRoute] string businessId, [FromRoute] string itemSku,
        [FromBody] CreateStockUpdateForm stockUpdateForm) {
        
        // Get our current UserId and BusinessId to validate and pass to the items service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string currentUserBusinessId = User.FindFirstValue("BusinessId");
        
        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }

        // Send Stock Update details to service to be validated and persisted
        CreateStockUpdateRequestDto createStockUpdateRequestDto = new CreateStockUpdateRequestDto(
             userId: currentUserId, 
             userBusinessId: currentUserBusinessId,
             businessId: businessId,
             itemSku: itemSku,
             changeStockAmountBy: stockUpdateForm.ChangeStockAmountBy,
             reasonForChange: stockUpdateForm.ReasonForChange);
        
        StockUpdateDto stockUpdateDto = await _itemStockService.CreateStockUpdate(createStockUpdateRequestDto);
        
        // Check for any errors when creating the Stock Update, return appropriately
        if (stockUpdateDto.ErrorNotification.HasErrors) {
            if (stockUpdateDto.ErrorNotification.Errors["otherErrors"].Contains(StockUpdateDto.USER_UNAUTHORISED_ERROR)) {
                return Unauthorized();
            }
            return new BadRequestObjectResult(stockUpdateDto.ErrorNotification);
        }
        
        // Return 201 created with object or appropriate response depending on errors
        string createdStockUpdateUrl = Url.Action(controller: "itemStock", action: nameof(GetStockUpdates),
            values: new { businessId=createStockUpdateRequestDto.BusinessId, itemSku=createStockUpdateRequestDto.ItemSku }, protocol: Request.Scheme) ?? "";

        return Created(createdStockUpdateUrl, stockUpdateDto);
    }
    
    [HttpGet]
    [Route("items/{itemSku}/stock/updates")]
    public async Task<IActionResult> GetStockUpdates([FromRoute] string businessId, [FromRoute] string itemSku) {
        throw new NotImplementedException();
    }
}