using System.Security.Claims;
using instock_server_application.Businesses.Controllers.forms;
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
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        

        ConnectionsService connectionService = new ConnectionsService(_businessRepository);

        GetConnectionsRequestDto getConnectionsRequestDto = new GetConnectionsRequestDto(
            userId : currentUserId!,
            userBusinessId: currentUserBusinessId!,
            businessId: businessId);
        

        var allConnections = await connectionService.GetConnections(getConnectionsRequestDto);
      
        return Ok(allConnections);
    }
    
    [HttpPost]
    [Route("connections")]
    public async Task<IActionResult> AddConnections([FromBody] CreateConnectionForm createConnectionForm, [FromRoute] string businessId) {
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        //Service pinging for mock server for auth token
        //
        ConnectionsService connectionService = new ConnectionsService(_businessRepository);

        Console.WriteLine("Calling");
        String res = await connectionService.ConnectToExternalShop(createConnectionForm);
        Console.WriteLine("Called");
        Console.WriteLine(res);
        
        
        CreateConnectionRequestDto createConnectionRequestDto = new CreateConnectionRequestDto(
            currentUserId!, 
            currentUserBusinessId!,
             businessId,
            createConnectionForm.ShopNameConnectingTo,
            "SampleToken123");
        var allConnections = await connectionService.CreateConnections(createConnectionRequestDto);
        Console.WriteLine("res");
        Console.WriteLine(allConnections);
        
        return Ok(allConnections);
    }
    
}
