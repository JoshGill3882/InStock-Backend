using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class BusinessConnectionService : IBusinessConnectionService {
    private IBusinessRepository _businessRepository;
    private IItemRepo _itemRepo;

    public BusinessConnectionService(IItemRepo itemRepo, IBusinessRepository businessRepository) {
        _itemRepo = itemRepo;
        _businessRepository = businessRepository;
    }

    public async void SyncAllBusinessesItemOrders(object? callingObject) {
        // TODO Get all BusinessIds
        // List<string> listOfBusinessIds
        
        List<ItemConnectionsDto> listOfConnections = await _itemRepo.GetAllItemsWithConnections("asd");

        // TODO Get all items with a connection
        // TODO Loop all items
            // TODO Loop the connection
                // TODO Create the ExternalConnector
                // TODO Get the live orders of the items
            // TODO Sum the total orders from connections
            // TODO Add order update of the difference between our total and the sum'd total
    }

    public void SyncAllBusinessesItemStock(object? callingObject) {
        Console.Out.WriteLine("ItemStock");
    }

}