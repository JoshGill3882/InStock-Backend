using System.Security.Claims;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Authorize]
public class BusinessController : ControllerBase {
    private IBusinessService _businessService;

    public BusinessController(IBusinessService businessService) {
        _businessService = businessService;
    }

    [HttpPost]
    [Route("/business")]
    public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessForm newBusinessForm) {

        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }

        // Creating CreateBusinessDto to pass the details to the service for processing
        CreateBusinessRequestDto businessRequestToCreate = new CreateBusinessRequestDto(newBusinessForm.BusinessName, 
            currentUserId, newBusinessForm.BusinessDescription);

        // Attempting to create new business, it returns success of failure
        BusinessDto createdBusiness = await _businessService.CreateBusiness(businessRequestToCreate);

        // If errors then return 401 with the error messages
        if (createdBusiness.ErrorNotification.HasErrors) {
            return new BadRequestObjectResult(createdBusiness.ErrorNotification);
        }
        
        // If not errors then return 201 with the URI and newly created object details
        string? createdBusinessUrl = Url.Action(controller: "business", action: nameof(GetBusiness), values:new {businessId=createdBusiness.BusinessId}, protocol:Request.Scheme);
        return Created(createdBusinessUrl ?? string.Empty, new {
            businessId = createdBusiness.BusinessId,
            businessName = createdBusiness.BusinessName,
            businessDescription = createdBusiness.BusinessDescription,
            businessOwnerId = createdBusiness.BusinessOwnerId
        });
    }

    [HttpGet]
    [Route("/businesses/{businessId}")]
    public async Task<IActionResult> GetBusiness([FromRoute] string businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;
        
        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        Dictionary<string, string> businessDetails = _businessService.GetBusiness(new BusinessDto(currentUserBusinessId, businessId)).Result;

        if (businessDetails == null) {
            return Unauthorized();
        } if (businessDetails.Count == 0) {
            return NotFound();
        }

        return Ok(businessDetails);
    }
}