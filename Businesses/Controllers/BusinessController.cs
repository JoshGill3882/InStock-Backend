using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using instock_server_application.Auth.Services.Interfaces;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Authorize]
public class BusinessController : ControllerBase {
    private readonly IBusinessService _businessService;
    private readonly IJwtService _jwtService;

    public BusinessController(IBusinessService businessService, IJwtService jwtService) {
        _businessService = businessService;
        _jwtService = jwtService;
    }

    [HttpPost]
    [Route("/business")]
    public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessForm newBusinessForm) {

        // Get our current User details from the ClaimsPrinciple Object
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserEmail = User.FindFirstValue("Email") ?? null;

        // Check there are no issues with the ID or Email
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserEmail)) {
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
        
        // If not errors then return 201 with the URI and newly created object details and a new JWT
        string? createdBusinessUrl = Url.Action(controller: "business", action: nameof(GetBusiness), values:new {businessId=createdBusiness.BusinessId}, protocol:Request.Scheme);
        string newJwtToken = _jwtService.CreateToken(currentUserId, currentUserEmail, createdBusiness.BusinessId);
        return Created(createdBusinessUrl ?? string.Empty, new {
            businessId = createdBusiness.BusinessId,
            businessName = createdBusiness.BusinessName,
            businessDescription = createdBusiness.BusinessDescription,
            businessOwnerId = createdBusiness.BusinessOwnerId,
            newJwtToken
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
        
        BusinessDto businessDetails = _businessService.GetBusiness(new ValidateBusinessIdDto(currentUserBusinessId, businessId)).Result;

        if (businessDetails.ErrorNotification.HasErrors) {
            if (businessDetails.ErrorNotification.Errors.ContainsKey("otherErrors")) {
                if (businessDetails.ErrorNotification.Errors["otherErrors"].Contains("Unauthorized")) {
                    return Unauthorized();
                }
            }
            return new BadRequestObjectResult(businessDetails.ErrorNotification);
        }

        return Ok(businessDetails);
    }
}