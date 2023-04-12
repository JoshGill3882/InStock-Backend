using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Util.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Authorize]
[Route("/statistics")]
public class StatisticsController : ControllerBase {
    
    private readonly IStatisticsService _statitisticsService;

    public StatisticsController(IStatisticsService statisticsService) {
        _statitisticsService = statisticsService;
    }

    /// <summary>
    /// Function for getting stats for a specific business, providing the currently logged in user has access
    /// </summary>
    /// <param name="businessId"> The businessId to get the stats for </param>
    /// <returns> A DTO containing stats for the business. Including overall performance, performance by
    /// category, and sales/deductions per month</returns>
    [HttpGet]
    [Route("{businessId}")]
    public async Task<IActionResult> GetStatistics([FromRoute] string businessId)
    {
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        // Creating new userDto to pass into service
        UserDto currentUserDto = new UserDto(currentUserId, currentUserBusinessId);
        
        AllStatsDto? stats = _statitisticsService.GetStats(currentUserDto, businessId).Result;

        if (stats == null)
        {
            return Unauthorized();
        }
        return Ok(stats);
    }
}