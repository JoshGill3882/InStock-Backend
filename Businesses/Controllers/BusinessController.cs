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
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }

        // Creating CreateBusinessDto to pass the details to the service for processing
        CreateBusinessRequestDto businessRequestToCreate = new CreateBusinessRequestDto(newBusinessForm.BusinessName, 
            currentUserId, currentUserBusinessId);

        // Attempting to create new business, it returns success of failure
        BusinessDto createdBusiness = await _businessService.CreateBusiness(businessRequestToCreate);
        string? createdBusinessUrl = Url.Action(controller: "business", action: nameof(GetBusiness), values:new {businessId=createdBusiness.BusinessId}, protocol:Request.Scheme);

        // If Success, return 201
        return Created(createdBusinessUrl ?? string.Empty, createdBusiness);
    }

    [Route("{businessId}")]
    [HttpGet]
    public async Task<IActionResult> GetBusiness(string businessId) {
        throw new NotImplementedException();
    }
}