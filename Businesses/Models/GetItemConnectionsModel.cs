using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace instock_server_application.Businesses.Models;

[DynamoDBTable("Items")]
public class GetItemConnectionsModel {
    private int _totalStock;
    private int _totalOrders;

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
    
    [DynamoDBProperty("TotalOrders")]
    public String TotalOrders {
        get => _totalOrders.ToString();
        set {
            if (value.Length > int.MaxValue.ToString().Length) {
                _totalOrders = value.Contains('-') ? int.MinValue : int.MaxValue;
            } else if ( long.Parse(value) >= int.MaxValue) {
                _totalOrders = int.MaxValue;
            } else if (long.Parse(value) <= int.MinValue) {
                _totalOrders = int.MinValue;
            }
            else {
                _totalOrders = int.Parse(value);
            }
        }
    }
    
    [DynamoDBProperty("ItemConnections")]
    public Dictionary<string, string> Connections { get; set; }
    
    public GetItemConnectionsModel() {
        Connections = new Dictionary<string, string>();

    }
    
    public GetItemConnectionsModel(string sku, string businessId, int totalStock, int totalOrders, Dictionary<string, string>? connections) {
        Sku = sku;
        BusinessId = businessId;
        _totalStock = totalStock;
        _totalOrders = totalOrders;
        Connections = connections ?? new Dictionary<string, string>();
    }
    
    public int GetTotalStock() {
        return _totalStock;
    }
    
    public int GetTotalOrders() {
        return _totalOrders;
    }

    // Scan conditions used in scans for this model
    public static ScanCondition ByValidConnection() {
        return new ScanCondition(nameof(Connections), ScanOperator.IsNotNull);
    }
}