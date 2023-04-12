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
    public async Task<IActionResult> GetBusiness([FromRoute] string email) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;
        
        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        User user = _userService.FindUserByEmail(email).Result;

        AccountDetailsDto accountDetailsDto = new AccountDetailsDto(
            firstName: user.FirstName,
            lastName: user.LastName,
            email: user.Email,
            imageUrl: user.ImageUrl
        );

        return Ok(accountDetailsDto);
    }
}