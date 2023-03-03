using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
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
    public async Task<IActionResult> GetAllItems([FromBody] BusinessIdModel businessIdModel) {
        // Get our current UserId and BusinessId to validate and pass to the business service
        string currentUserId = User.FindFirstValue("Id");
        string currentUserBusinessId = User.FindFirstValue("BusinessId").Split(",")[0];

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }

        // Creating new userDto to pass into service
        UserDto currentUserDto = new UserDto(currentUserId, currentUserBusinessId);
        
        List<Dictionary<string, string>>? items = _itemService.GetItems(currentUserDto, businessIdModel.BusinessId).Result;
        
        if (items == null) {
            return Unauthorized();
        } if (items.Count == 0) {
            return NotFound();
        }

        return Ok(items);
    }
}