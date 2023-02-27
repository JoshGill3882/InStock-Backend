using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Authorize]
[Route("/business")]
public class BusinessController : ControllerBase {
    private IBusinessService _businessService;

    public BusinessController(IBusinessService businessService) {
        _businessService = businessService;
    }

    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessDto newBusinessDto) {

        // Get our current UserId and BusinessId to validate and pass to the business service
        string currentUserId = User.FindFirstValue("Id");
        string currentUserBusinessId = User.FindFirstValue("BusinessId");

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }

        // Creating new userDto to pass into service
        UserDto currentUserDto = new UserDto(currentUserId, currentUserBusinessId);

        // Attempting to create new business, it returns success of failure
        bool serviceResponse = _businessService.CreateBusiness(currentUserDto, newBusinessDto);

        // If Success, return 200
        if (serviceResponse) {
            return Ok();
        }

        // If fail, return 400
        return BadRequest();
    }
}