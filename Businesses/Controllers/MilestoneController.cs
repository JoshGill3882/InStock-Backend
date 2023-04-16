using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Route("/milestones")]
public class MilestoneController : ControllerBase {
    private readonly IMilestoneService _milestoneService;

    public MilestoneController(IMilestoneService milestoneService) {
        _milestoneService = milestoneService;
    }
    
    [HttpGet]
    [Route("{businessId}")]
    public async Task<IActionResult> GetMilestones([FromRoute] string businessId)
    {
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }

        ListOfMilestonesDto listOfMilestonesDto = _milestoneService.GetAllMilestones(businessId).Result;

        if (listOfMilestonesDto.ErrorNotification.Errors.ContainsKey("otherErrors")) {
            if (listOfMilestonesDto.ErrorNotification.Errors["otherErrors"].Contains(ListOfItemDto.ERROR_UNAUTHORISED)) {
                return Unauthorized();
            }
        }
        
        if (listOfMilestonesDto.ListOfMilestones.Count <= 0) {
            return NotFound();
        }
        
        return Ok(listOfMilestonesDto.ListOfMilestones);
    }
}