using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Abstractions;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class BusinessConnectionService : IBusinessConnectionService {
    private IItemRepo _itemRepo;
    private IItemOrderService _itemOrderService;

    public BusinessConnectionService(IItemRepo itemRepo, IItemOrderService itemOrderService) {
        _itemRepo = itemRepo;
        _itemOrderService = itemOrderService;
    }

    public async void SyncAllBusinessesItemsToConnections(object? callingObject) {
        
        // Get all items with a connection
        List<ItemSyncConnectionsDto> listOfConnections;
        try {
            listOfConnections = await _itemRepo.GetAllItemsWithConnections();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return;
        }

        // Looping ever item
        foreach (ItemSyncConnectionsDto connection in listOfConnections) {
            int totalOrderSum = 0;
            // Looping each connection within an item
            foreach (string platformName in connection.Connections.Keys) {
                ExternalShopConnectorService externalConnection = ExternalServiceConnectorFactory.CreateConnector(platformName);
                
                // Getting the connected item's details and adding it to the sum
                ConnectedItemDetailsDto connectedItemDetails = await externalConnection.GetConnectedItemDetails(connection.Sku);
                totalOrderSum += Int32.Parse(connectedItemDetails.TotalOrders);
                
                // Setting the shops stock to match the businesses inventory
                // ItemDto itemDto = await _itemRepo.GetItem(connection.BusinessId, connection.Sku) ?? new ItemDto("","","","",0,0,0,"");
                externalConnection.SetItemStock(connection.BusinessId, connection.Sku, connection.TotalStock);
            }
            // Saving the new total orders of the item
            _itemOrderService.SetItemTotalOrders(connection.BusinessId, connection.Sku, totalOrderSum);
        }
    }

}