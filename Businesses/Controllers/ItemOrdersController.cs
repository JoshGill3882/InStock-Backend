using System.Security.Claims;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Route("/businesses/{businessId}/items/{itemSku}/")]
public class ItemOrdersController : ControllerBase {
    private readonly IItemOrderService _itemOrderService;
    
    public ItemOrdersController(IItemOrderService itemOrderService) {
        _itemOrderService = itemOrderService;
    }
    
    [HttpPost]
    [Route("orders")]
    public async Task<IActionResult> CreateItemOrders([FromRoute] string businessId, [FromRoute] string itemSku,
        [FromBody] CreateItemOrderForm itemOrderForm) {
        
        // Get our current UserId and BusinessId to validate and pass to the items service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string currentUserBusinessId = User.FindFirstValue("BusinessId");
        
        // Check there are no issues with the userId or BusinessId
        if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        // Converting Amount string to int, done here instead of within form so we can control the error message
        int amountOrdered;
        bool isInt = int.TryParse(itemOrderForm.AmountOrdered, out amountOrdered);
        if (!isInt) {
            var response = new ErrorNotification();
            response.AddError("AmountOrdered", $"The order amount must be a valid number no larger than {int.MaxValue} and no smaller than {int.MinValue}.");
            return new BadRequestObjectResult(response);
        }

        // Send Item Order details to service to be validated and persisted
        CreateItemOrderRequestDto createItemOrderRequestDto = new CreateItemOrderRequestDto(
            userId: currentUserId,
            userBusinessId: currentUserBusinessId,
            businessId: businessId,
            itemSku: itemSku,
            amountOrdered: amountOrdered);
        
        ItemOrderDto itemOrderDto = await _itemOrderService.CreateItemOrder(createItemOrderRequestDto);
        
        // Check for any errors when creating the item order, return appropriately
        if (itemOrderDto.ErrorNotification.HasErrors) {
            if (itemOrderDto.ErrorNotification.Errors.ContainsKey("otherErrors")) {
                if (itemOrderDto.ErrorNotification.Errors["otherErrors"].Contains(ItemOrderDto.USER_UNAUTHORISED_ERROR)) {
                    return Unauthorized();
                }
            }
            return new BadRequestObjectResult(itemOrderDto.ErrorNotification);
        }
        
        // Return 201 created with object or appropriate response depending on errors
        string getItemOrderUrl = Url.Action(controller: "itemOrders", action: nameof(GetItemOrders),
            values: new { businessId=createItemOrderRequestDto.BusinessId, itemSku=createItemOrderRequestDto.ItemSku }, protocol: Request.Scheme) ?? "";

        return Created(getItemOrderUrl, itemOrderDto);
    }
    
    [HttpGet]
    [Route("orders")]
    public async Task<IActionResult> GetItemOrders([FromRoute] string businessId, [FromRoute] string itemSku) {
        throw new NotImplementedException();
    }
}