using instock_server_application.Items.Models;

namespace instock_server_application.Items.Services.Interfaces;

public interface IItemService {
    public Task<List<Dictionary<string, string>>?> GetItems(string businessId);
}