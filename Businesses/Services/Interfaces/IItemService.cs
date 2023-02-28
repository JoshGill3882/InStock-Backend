using System.Security.Claims;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Services.Interfaces;

public interface IItemService {
    public Task<List<Dictionary<string, string>>?> GetItems(ClaimsPrincipal User, string businessId);
}