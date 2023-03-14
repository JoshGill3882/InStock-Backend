using System.Security.Claims;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

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
    /// <returns> List of all the Items found, or an error message with a 404 status code </returns>
    [HttpGet]
    [Route("items")]
    public async Task<IActionResult> GetAllItems([FromRoute] string businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string currentUserBusinessId = User.FindFirstValue("BusinessId").Split(",")[0];

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
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
    
    // Route to maintain old implementation
    // not permanent
    [HttpGet]
    [Route("/items")]
    public async Task<IActionResult> GetAllItemsWithParam(string businessId)
    {
        return await GetAllItems(businessId);
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

    [HttpPost]
    [Route("items/{itemId}")]
    public async Task<IActionResult> DeleteItem([FromRoute] string itemId, [FromRoute] string businessId) {
        string? response = _itemService.DeleteItem(new DeleteItemDto(User, itemId, businessId)).Result;

        if (string.IsNullOrEmpty(response)) {
            return Unauthorized();
        }

        return Ok(response);
    }
}