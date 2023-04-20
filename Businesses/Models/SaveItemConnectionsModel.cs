using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace instock_server_application.Businesses.Models;

[DynamoDBTable("Items")]
public class SaveItemConnectionsModel {

    [DynamoDBHashKey]
    [DynamoDBProperty("SKU")]
    public string Sku { get; set; }
    
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }

    [DynamoDBProperty("ItemConnections")]
    public Dictionary<string, string> Connections { get; set; }
    
    public SaveItemConnectionsModel() {
        Connections = new Dictionary<string, string>();

    }
    
    public SaveItemConnectionsModel(string sku, string businessId, Dictionary<string, string>? connections) {
        Sku = sku;
        BusinessId = businessId;
        Connections = connections ?? new Dictionary<string, string>();
    }
}