using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Services.Interfaces;

public interface IItemService {
    public Task<List<Item>?> GetItems(string businessId);
}