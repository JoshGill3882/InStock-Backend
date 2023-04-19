using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Models;

public class ItemSyncConnectionsDto : DataTransferObjectSuperType {

    public string Sku { get; }

    public string BusinessId { get; }
    
    public int AvailableStock { get; }
    public Dictionary<string, string> Connections { get; }

    public ItemSyncConnectionsDto(string sku, string businessId, int availableStock, Dictionary<string, string> connections) {
        Sku = sku;
        BusinessId = businessId;
        AvailableStock = availableStock;
        Connections = connections;
    }
    
    public ItemSyncConnectionsDto(ErrorNotification errorNotes) : base(errorNotes) { }
}