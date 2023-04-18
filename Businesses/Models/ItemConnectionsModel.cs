using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace instock_server_application.Businesses.Models;

[DynamoDBTable("Items")]
public class ItemConnectionsModel {
    private int _totalStock;

    [DynamoDBHashKey]
    [DynamoDBProperty("SKU")]
    public string Sku { get; set; }
    
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }
    
    [DynamoDBProperty("Stock")]
    public String TotalStock {
        get => _totalStock.ToString();
        set {
            if (value.Length > int.MaxValue.ToString().Length) {
                _totalStock = value.Contains('-') ? int.MinValue : int.MaxValue;
            } else if ( long.Parse(value) >= int.MaxValue) {
                _totalStock = int.MaxValue;
            } else if (long.Parse(value) <= int.MinValue) {
                _totalStock = int.MinValue;
            }
            else {
                _totalStock = int.Parse(value);
            }
        }
    }
    
    [DynamoDBProperty("ItemConnections")]
    public Dictionary<string, string> Connections { get; set; }
    
    public ItemConnectionsModel() {
        Connections = new Dictionary<string, string>();

    }
    
    public ItemConnectionsModel(string sku, string businessId, Dictionary<string, string>? connections) {
        Sku = sku;
        BusinessId = businessId;
        Connections = connections ?? new Dictionary<string, string>();
    }
    
    public int GetTotalStock() {
        return _totalStock;
    }

    // Scan conditions used in scans for this model
    public static ScanCondition ByValidConnection() {
        return new ScanCondition(nameof(Connections), ScanOperator.IsNotNull);
    }
}