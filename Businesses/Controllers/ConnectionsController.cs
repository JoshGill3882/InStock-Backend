using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Route("/businesses/{businessId}")]
public class ConnectionsController : ControllerBase {
    
    [HttpGet]
    [Route("connections")]
    public async Task<IActionResult> getAllConnections([FromRoute] string businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        //Code to get all current connections
        
        // Creating new userDto to pass into service
        // UserDto currentUserDto = new UserDto(currentUserId, currentUserBusinessId);
        
        // List<Dictionary<string, string>>? items = _itemService.GetItems(currentUserDto, businessId).Result;

        // if (items == null) {
            // return Unauthorized();
        // } if (items.Count == 0) {
            // return NotFound();
        // }

        return Ok("Nice");
    }
    
    [HttpPost]
    [Route("connections")]
    public async Task<IActionResult> addConnections([FromRoute] string businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        //Code to get all current connections
        
        // Creating new userDto to pass into service
        // UserDto currentUserDto = new UserDto(currentUserId, currentUserBusinessId);
        
        // List<Dictionary<string, string>>? items = _itemService.GetItems(currentUserDto, businessId).Result;

        // if (items == null) {
        // return Unauthorized();
        // } if (items.Count == 0) {
        // return NotFound();
        // }

        return Ok("Posting");
    }
    
}