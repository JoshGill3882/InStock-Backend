using System.Security.Claims;
using instock_server_application.Businesses.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class BusinessService : IBusinessService {
    /// <summary>
    /// Function for checking if a given BusinessId is contained within a User's Claims
    /// </summary>
    /// <param name="User"> The currently logged in User </param>
    /// <param name="idToCheck"> The ID to check for </param>
    /// <returns> True/False depending if the ID is found </returns>
    public bool CheckBusinessIdInJWT(ClaimsPrincipal User, string idToCheck) {
        // Get the claims of a User, and seperate the BusinessIds into an array of string
        string businessIds = User.FindFirstValue("BusinessIds");
        string[] ids = businessIds.Split(",");
        
        // If the array contains the search param, return true
        // Otherwise, return false
        if (ids.Contains(idToCheck)) {
            return true;
        }
        return false;
    }
}