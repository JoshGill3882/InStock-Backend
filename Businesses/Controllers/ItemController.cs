using System.Security.Claims;
using System.Text.RegularExpressions;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Route("/items")]
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
    [Route("/items")]
    public async Task<IActionResult> GetAllItems(String businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string currentUserId = User.FindFirstValue("Id");
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

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemForm newItemForm) {

        // Get our current UserId and BusinessId to validate and pass to the items service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        
        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }

        // Creating CreateItemDTO to pass the details to the service for processing
        CreateItemRequestDto createItemRequestDto = new CreateItemRequestDto(newItemForm.SKU,
            newItemForm.BusinessId, newItemForm.Category, newItemForm.Name, newItemForm.Stock, currentUserId);

        // Attempting to create new item
        ItemDto createdItemDTO = await _itemService.CreateItem(createItemRequestDto);

        // If errors then return 401 with the error messages
        if (createdItemDTO.ErrorNotification.HasErrors) {
            return new BadRequestObjectResult(createdItemDTO.ErrorNotification);
        }
        
        // If not errors then return 201 with the URI and newly created object details
        string? createdItemUrl = Url.Action(controller: "item", action: nameof(GetItem), values:new {itemId=createdItemDTO.SKU}, protocol:Request.Scheme);
        return Created(createdItemUrl ?? string.Empty, new {
            sku = createdItemDTO.SKU,
            businessId = createdItemDTO.BusinessId,
            category = createdItemDTO.Category,
            name = createdItemDTO.Name,
            stock = createdItemDTO.Stock
        });
    }
    
    [Route("{itemId}")]
    [HttpGet]
    public async Task<IActionResult> GetItem(string itemId) {
        throw new NotImplementedException();
    }


}