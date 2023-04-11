using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services;
using instock_server_application.Businesses.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Route("/businesses/{businessId}")]
public class ConnectionsController : ControllerBase {
    
    private readonly IBusinessRepository _businessRepository;
        
    public ConnectionsController(IBusinessRepository businessRepository) {
        _businessRepository = businessRepository;
    }
    
    [HttpGet]
    [Route("connections")]
    public async Task<IActionResult> GetAllConnections([FromRoute] string businessId) {
        Console.WriteLine("Its working");
        Console.WriteLine(businessId);
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
    public async Task<IActionResult> AddConnections([FromRoute] string businessId) {
        Console.WriteLine("This is working too");
        Console.WriteLine(businessId);
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        //Code to get all current connections
        
        // Creating CreateItemDTO to pass the details to the service for processing
        // CreateItemRequestDto createItemRequestDto = new CreateItemRequestDto(
        //     newItemForm.SKU,
        //     businessId, 
        //     newItemForm.Category, 
        //     newItemForm.Name, 
        //     newItemForm.Stock, 
        //     currentUserId,
        //     newItemForm.ImageFile
        // );
        //
        // ItemDto createdItemDto = await _itemService.CreateItem(createItemRequestDto);
      

        ConnectionsService connectionService = new ConnectionsService(_businessRepository);
        CreateConnectionRequestDto createConnectionRequestDto = new CreateConnectionRequestDto(
            "34ddad47-45ad-429d-97d9-5095d310e815", 
            "6b416f45-2ac1-462f-b6d8-94fef82e5925",
            "6b416f45-2ac1-462f-b6d8-94fef82e5925",
            "Hello",
            "Hello");
        var allConnections = await connectionService.CreateConnection(createConnectionRequestDto);
        Console.WriteLine("res");
        Console.WriteLine(allConnections);
        
        return Ok(allConnections);
    }
    
}