using System.Security.Claims;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Route("/businesses/{businessId}")]
public class ItemController : ControllerBase {
    private readonly IItemService _itemService;
    
    public ItemController(IItemService itemService) {
        _itemService = itemService;
    }

    /// <summary>
    /// Function for getting all the items for a specific business, providing the currently logged in user has access
    /// </summary>
    /// <param name="businessIdModel"> The BusinessID to get all the items for </param>
    /// <returns> List of all the Items found, or an error message with a 404 status code </returns>
    [HttpGet]
    [Route("items")]
    public async Task<IActionResult> GetAllItems([FromRoute] string businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        // Creating new userDto to pass into service
        UserDto currentUserDto = new UserDto(currentUserId, currentUserBusinessId);
        
        List<Dictionary<string, string>>? items = _itemService.GetItems(currentUserDto, businessId).Result;

        if (items == null) {
            return Unauthorized();
        } if (items.Count == 0) {
            return NotFound();
        }

        return Ok(items);
    }

    /// <summary>
    /// Add item for a specific business
    /// </summary>
    /// <param name="newItemForm"> Create item form </param>
    /// <param name="businessId"> Unique ID for the business the item needs to be added to</param>
    /// <returns> Item created, or error with relevant status code</returns>
    [HttpPost]
    [Route("items")]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemForm newItemForm, [FromRoute] string businessId) {

        // Get our current UserId and BusinessId to validate and pass to the items service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        
        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }

        // Creating CreateItemDTO to pass the details to the service for processing
        CreateItemRequestDto createItemRequestDto = new CreateItemRequestDto(newItemForm.SKU,
            businessId, newItemForm.Category, newItemForm.Name, newItemForm.Stock, currentUserId);

        // Attempting to create new item
        ItemDto createdItemDTO = await _itemService.CreateItem(createItemRequestDto);

        // If errors then return 401 with the error messages
        if (createdItemDTO.ErrorNotification.HasErrors) {
            return new BadRequestObjectResult(createdItemDTO.ErrorNotification);
        }
        
        // If not errors then return 201 with the URI and newly created object details
        string? createdItemUrl = Url.Action(controller: "item", action: nameof(GetItem), values:new
        {
            businesses=Url.RouteUrl("businesses"),
            businessId = createdItemDTO.BusinessId,
            items=Url.RouteUrl("items"),
            itemId=createdItemDTO.SKU
        }, protocol:Request.Scheme);
        return Created(createdItemUrl ?? string.Empty, new {
            sku = createdItemDTO.SKU,
            businessId = createdItemDTO.BusinessId,
            category = createdItemDTO.Category,
            name = createdItemDTO.Name,
            stock = createdItemDTO.Stock
        });
    }
    
    [HttpGet]
    [Route("items/{itemId}")]
    public async Task<IActionResult> GetItem([FromRoute] string itemId, string businessId) {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("items/{itemId}")]
    public async Task<IActionResult> DeleteItem([FromRoute] string itemId, [FromRoute] string businessId) {
        DeleteItemDto result = _itemService.DeleteItem(new DeleteItemDto(itemId, User.FindFirstValue("BusinessId"), businessId)).Result;

        if (result.ErrorNotification.HasErrors) {
            if (result.ErrorNotification.Errors["otherErrors"].Contains(DeleteItemDto.USER_UNAUTHORISED_ERROR)) {
                return Unauthorized();
            }
            return new BadRequestObjectResult(result.ErrorNotification.Errors);
        }
        return Ok();
    }
    
    /// <summary>
    /// Function for getting all the categories for a specific business, providing the currently logged in user has access
    /// </summary>
    /// <param name="businessIdModel"> The BusinessID to get all the categories for </param>
    /// <returns> List of all the Categories found, or an error message with a 404 status code </returns>
    [HttpGet]
    [Route("categories")]
    public async Task<IActionResult> GetAllCategories([FromRoute] string businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }

        List<Dictionary<string, string>>? categories = _itemService.GetCategories(new CategoryDto(currentUserBusinessId, businessId)).Result;

        if (categories == null) {
            return Unauthorized();
        } if (categories.Count == 0) {
            return NotFound();
        }

        return Ok(categories);
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
        
        StockUpdateDto stockUpdateDto = await _itemService.CreateStockUpdate(createStockUpdateRequestDto);
        
        // Check for any errors when creating the Stock Update, return appropriately
        if (stockUpdateDto.ErrorNotification.HasErrors) {
            return new BadRequestObjectResult(stockUpdateDto.ErrorNotification);
        }
        
        // Return 201 created with object or appropriate response depending on errors
        string createdStockUpdateUrl = Url.Action(controller: "item", action: nameof(GetStockUpdates),
            values: new { businessId=createStockUpdateRequestDto.BusinessId, itemSku=createStockUpdateRequestDto.ItemSku }, protocol: Request.Scheme) ?? "";

        return Created(createdStockUpdateUrl, stockUpdateDto);
    }
    
    [HttpGet]
    [Route("businesses/{businessId}/items/{itemSku}/stock/updates")]
    public async Task<IActionResult> GetStockUpdates([FromRoute] string businessId, [FromRoute] string itemSku) {
        throw new NotImplementedException();
    }
}