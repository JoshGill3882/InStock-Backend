using System.Security.Claims;
using instock_server_application.Businesses.Controllers.forms;
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
    public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessForm newBusinessForm) {

        // Get our current UserId and BusinessId to validate and pass to the business service
        string currentUserId = User.FindFirstValue("Id");
        string currentUserBusinessId = User.FindFirstValue("BusinessId");

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }

        // Creating CreateBusinessDto to pass the details to the service for processing
        CreateBusinessRequestDto businessRequestToCreate = new CreateBusinessRequestDto(newBusinessForm.BusinessName, 
            currentUserId, currentUserBusinessId);

        // Attempting to create new business, it returns success of failure
        bool serviceResponse = await _businessService.CreateBusiness(businessRequestToCreate);

        // If Success, return 200
        // TODO Return 201 Created
        if (serviceResponse) {
            return Ok();
        }

        // If fail, return 400
        return BadRequest();
    }
}