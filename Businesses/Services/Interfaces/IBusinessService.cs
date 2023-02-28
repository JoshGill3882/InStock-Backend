using System.Security.Claims;

namespace instock_server_application.Businesses.Services.Interfaces; 

public interface IBusinessService {
    public bool CheckBusinessIdInJWT(ClaimsPrincipal User, string idToCheck);
}