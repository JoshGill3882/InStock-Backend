using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Authorize]
[Route("/statistics")]
public class StatisticsController : ControllerBase {
    
    //Constructor not needed yet as this is just a mock that doesnt depend on services

    /// <summary>
    /// Mock Function for getting stats which provide an overview for a businesses performance.
    /// </summary>
    /// <param name="businessIdModel"> The BusinessID to get stats for </param>
    /// <returns> An overview , or an error message with a 404 status code </returns>
    [HttpGet]
    [Route("{businessId}")]
    public async Task<IActionResult> GetItem([FromRoute] string businessId) {

        // Check user and business are both valid
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        Dictionary<string, object> stats = new Dictionary<string, object>();
        Dictionary<string, int> overallShopPerformance = new Dictionary<string, int>();
        overallShopPerformance.Add("orders", 102);
        overallShopPerformance.Add("corrections", 6);
        overallShopPerformance.Add("returns", 2);
        overallShopPerformance.Add("giveaways", 10);
        overallShopPerformance.Add("damaged", 1);
        overallShopPerformance.Add("restocked", 50);
        overallShopPerformance.Add("lost", 0);
        
        stats.Add("overallShopPerformance", overallShopPerformance);
        
        Dictionary<string, object> performanceByCategory = new Dictionary<string, object>();
        
        Dictionary<string, int> cardsPerformance = new Dictionary<string, int>();
        cardsPerformance.Add("orders", 42);
        cardsPerformance.Add("corrections", 5);
        cardsPerformance.Add("returns", 0);
        cardsPerformance.Add("giveaways", 5);
        cardsPerformance.Add("damaged", 1);
        cardsPerformance.Add("restocked", 30);
        cardsPerformance.Add("lost", 0);
        performanceByCategory.Add("cards", cardsPerformance);
        
        Dictionary<string, int> stickersPerformance = new Dictionary<string, int>();
        stickersPerformance.Add("orders", 24);
        stickersPerformance.Add("corrections", 1);
        stickersPerformance.Add("returns", 2);
        stickersPerformance.Add("giveaways", 3);
        stickersPerformance.Add("damaged", 0);
        stickersPerformance.Add("restocked", 20);
        stickersPerformance.Add("lost", 0);
        performanceByCategory.Add("stickers", stickersPerformance);
        
        Dictionary<string, int> bookmarksPerformance = new Dictionary<string, int>();
        bookmarksPerformance.Add("orders", 36);
        bookmarksPerformance.Add("corrections", 0);
        bookmarksPerformance.Add("returns", 0);
        bookmarksPerformance.Add("giveaways", 2);
        bookmarksPerformance.Add("damaged", 0);
        bookmarksPerformance.Add("restocked", 0);
        bookmarksPerformance.Add("lost", 0);
        performanceByCategory.Add("bookmarks", bookmarksPerformance);
        stats.Add("performanceByCategory", performanceByCategory);
        
        // // Sales for past 6 months 
        Dictionary<string, int> salesByMonth = new Dictionary<string, int>()
        {
            {"October", 10},
            {"November", 20},
            {"December", 15},
            {"January", 12},
            {"February", 25},
            {"March", 20}
        };
        stats.Add("salesByMonth", salesByMonth);
        
        // Deductions for past 6 months
        Dictionary<string, int> deductionsByMonth = new Dictionary<string, int>()
        {
            {"October", 2},
            {"November", 4},
            {"December", 3},
            {"January", 1},
            {"February", 5},
            {"March", 5}
        };
        stats.Add("deductionsByMonth", deductionsByMonth);

        
        return Ok(stats);
    }
}