using instock_server_application.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Authorize]
[Route("/statistics")]
public class StatisticsController {
    
    //Constructor not needed yet as this is just a mock that doesnt depend on services

    /// <summary>
    /// Mock Function for getting stats which provide an overview for a businesses performance.
    /// </summary>
    /// <param name="businessIdModel"> The BusinessID to get stats for </param>
    /// <returns> An overview , or an error message with a 404 status code </returns>
    [HttpGet]
    [Route("businesses/statistics/{businessId}")]
    public async Task<IActionResult> GetItem([FromRoute] string businessId) {

        // Check user and business are both valid
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }

        Dictionary<string, object> stats = new Dictionary<string, object>() {
            {
                "totals", new Dictionary<string, int>() {
                    { "totalItemsOrdered", 16 },
                    { "totalItemsReturned", 5 },
                    { "totalItemsDamaged", 1 },
                    { "totalItemsSold", 47 },
                    { "totalItemsCorrectedInRecount", 3 },
                    { "totalItemsResent", 12 },
                    { "totalItemsGivenAway", 14 }
                }
            },
            {
                "advice", new Dictionary<string, string>() {
                    { "adviceForUser1", "You're having to resend alot of items, do you need package orders more securely?" },
                    { "adviceForUser2", "Cards appear to be your most popular product, keep up the good work!" }
                }
            },
            {
                "ordersByCategory", new Dictionary<string, int>() {
                    { "Cards", 8 },
                    { "Candles", 4 },
                    { "MaxWelts", 3 },
                    { "Stickers", 1 }
                }
            },
            {
                "salesByCategory", new Dictionary<string, int>() {
                    { "Cards", 22 },
                    { "Candles", 15 },
                    { "MaxWelts", 5 },
                    { "Stickers", 5 }
                }
            }
        };

        return Ok(stats);
    }
    
}