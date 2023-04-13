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
    
    private readonly IConnectionsService _connectionsService;

        
    public ConnectionsController(IConnectionsService connectionsService) {
        _connectionsService = connectionsService;
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
        GetConnectionsRequestDto getConnectionsRequestDto = new GetConnectionsRequestDto(
            userId : currentUserId!,
            userBusinessId: currentUserBusinessId!,
            businessId: businessId);


        StoreConnectionDto allConnections = await _connectionsService.GetConnections(getConnectionsRequestDto);
        if (allConnections.ErrorNotification.HasErrors) {
            if (allConnections.ErrorNotification.Errors.ContainsKey("otherErrors")) {
                if (allConnections.ErrorNotification.Errors["otherErrors"].Contains("Unauthorized")) {
                    return Unauthorized();
                }
            }
            return new BadRequestObjectResult(allConnections.ErrorNotification);
        }
        

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
        ExternalShopAuthenticationTokenDto authenticationToken = await _connectionsService.ConnectToExternalShop(createConnectionForm);

        if (authenticationToken.ErrorNotification.HasErrors) {
            if (authenticationToken.ErrorNotification.Errors.ContainsKey("otherErrors")) {
                if (authenticationToken.ErrorNotification.Errors["otherErrors"].Contains("Unauthorized")) {
                    return Unauthorized();
                }
            }
            return new BadRequestObjectResult(authenticationToken.ErrorNotification);
        }
        
        CreateConnectionRequestDto createConnectionRequestDto = new CreateConnectionRequestDto(
            userId: currentUserId!, 
            userBusinessId: currentUserBusinessId!,
             businessId: businessId,
            platformName: createConnectionForm.PlatformNameConnectingTo,
            authenticationToken: authenticationToken.AuthenticationToken,
            shopUsername: createConnectionForm.ShopUsername
            );
        
        
        StoreConnectionDto allConnections = await _connectionsService.CreateConnections(createConnectionRequestDto);
      
        if (allConnections.ErrorNotification.HasErrors) {
            if (allConnections.ErrorNotification.Errors.ContainsKey("otherErrors")) {
                if (allConnections.ErrorNotification.Errors["otherErrors"].Contains("Unauthorized")) {
                    return Unauthorized();
                }
            }
            return new BadRequestObjectResult(allConnections.ErrorNotification);
        }
        
        return Ok(allConnections);
    }
    
}
