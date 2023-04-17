using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class BusinessConnectionService : IBusinessConnectionService {
    public void SyncAllBusinessesItemOrders(object? callingObject) {
        Console.Out.WriteLine("ItemOrders");
    }

    public void SyncAllBusinessesItemStock(object? callingObject) {
        Console.Out.WriteLine("ItemStock");
    }
}