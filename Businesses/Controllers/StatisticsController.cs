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
        
        List<Dictionary<string, object>> stats = GenerateRandomDictionaries();
        
        
        return Ok(stats);
    }
    
    public static List<Dictionary<string, object>> GenerateRandomDictionaries()
    {
        List<Dictionary<string, object>> dictionaries = new List<Dictionary<string, object>>();

        // Define the possible values for the ReasonForChange key
        string[] reasonsForChange = { "Sale", "Restock", "Correction", "Damaged", "Returned", "Resent", "Giveaway", "Lost" };

        // Define the start and end dates for the DateTimeAdded key
        DateTime startDate = new DateTime(DateTime.Now.Year, 1, 1);
        DateTime endDate = new DateTime(DateTime.Now.Year, 12, 31);

        Random rand = new Random();

        // Generate 100 random dictionaries
        for (int i = 0; i < 100; i++)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            
            dictionary.Add("Sku", "BKM-" + (rand.Next(899) + 100));
            
            // Generate a random ReasonForChange
            dictionary.Add("ReasonForChange", reasonsForChange[rand.Next(reasonsForChange.Length)]);

            //Selects a random 
            List<string> items = new List<string>() {
                "Cards",
                "Candles",
                "Stickers",
                "Magnets",
                "Bookmarks"
            };

            Random random = new Random();
            string selected_cateogry = items[random.Next(items.Count)];
            
            dictionary.Add("Category", selected_cateogry);
            
            // Generate a random ItemName
            dictionary.Add("ItemName", "Item " + (i + 1));

            // Generate a random DateTimeAdded between the start and end dates
            TimeSpan timeSpan = endDate - startDate;
            TimeSpan newSpan = new TimeSpan(0, rand.Next(0, (int)timeSpan.TotalMinutes), 0);
            dictionary.Add("DateTimeAdded", startDate + newSpan);

            // Generate a random AmountChanged between -10 and 10
            dictionary.Add("Amount Changed", rand.Next(-10, 11));

            dictionaries.Add(dictionary);
        }

        return dictionaries;
    }
    
}