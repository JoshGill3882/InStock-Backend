using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace instock_server_application.Businesses.Models;

[DynamoDBTable("Items")]
public class ItemConnections {
    [DynamoDBHashKey]
    [DynamoDBProperty("SKU")]
    public string Sku { get; set; }
    
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }
    
    [DynamoDBProperty("ItemConnections")]
    public Dictionary<string, string> Connections { get; set; }
    
    public ItemConnections() {
    }
    
    public ItemConnections(string sku, string businessId, Dictionary<string, string> connections) {
        Sku = sku;
        BusinessId = businessId;
        Connections = connections;
    }

    // Scan conditions used in scans for this model
    public static ScanCondition ByBusinessId(string businessId) {
        return new ScanCondition(nameof(BusinessId), ScanOperator.Equal, businessId);
    }
}