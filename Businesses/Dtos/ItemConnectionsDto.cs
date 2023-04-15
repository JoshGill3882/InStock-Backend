using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace instock_server_application.Businesses.Models;

public class ItemConnectionsDto {

    public string Sku { get; }

    public string BusinessId { get; }
    
    public Dictionary<string, string> Connections { get; }

    public ItemConnectionsDto(string sku, string businessId, Dictionary<string, string> connections) {
        Sku = sku;
        BusinessId = businessId;
        Connections = connections;
    }
}