using System.Security.Claims;
using instock_server_application.Users.Dtos;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Users.Controllers; 

[ApiController]
public class UserController : ControllerBase {
    private readonly IUserService _userService;

    public UserController(IUserService userService) {
        _userService = userService;
    }
    
    [HttpGet]
    [Route("/users/{email}")]
    public async Task<IActionResult> GetUser([FromRoute] string email) {
        
        // Get our current UserId to validate and pass to the user service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        
        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }
        
        AccountDetailsDto accountDetailsDto = _userService.GetUser(email).Result;

        return Ok(accountDetailsDto);
    }
}