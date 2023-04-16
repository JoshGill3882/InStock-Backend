using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace instock_server_application.Businesses.Models;

[DynamoDBTable("Items")]
public class ItemStatsDetailsModel {
    // The value for the items stock, this is so we get more control over the value
    private int _totalStock;
    
    [DynamoDBHashKey]
    [DynamoDBProperty("SKU")]
    public string SKU { get; set; }
    
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }
    
    [DynamoDBProperty("Category")]
    public string Category { get; set; }
    
    [DynamoDBProperty("Name")]
    public string Name { get; set; }
    
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
    
    [DynamoDBProperty(typeof(ItemStockUpdateModel.StockUpdateObjectConverter))]
    public List<ItemStockUpdateModel.StockUpdateObject>? StockUpdates { get; set; }
    
    
    public ItemStatsDetailsModel() {
    }

    /// <summary>
    /// All Args Constructor
    /// </summary>
    /// <param name="sku"> Item's Unique Key </param>
    /// <param name="businessId"> Item's Business Id </param>
    /// <param name="category"> Item's Category </param>
    /// <param name="name"> Item's Name </param>
    /// <param name="totalStock">Item's Stock Level</param>
    /// <param name="stockUpdates">Item's list of updates to stock</param>
    public ItemStatsDetailsModel(string sku, string businessId, string category, string name, int totalStock, List<ItemStockUpdateModel.StockUpdateObject> stockUpdates) {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        _totalStock = totalStock;
        StockUpdates = stockUpdates;
    }

    public int GetTotalStock() {
        return _totalStock;
    }

    // Scan conditions used in scans for this model
    public static ScanCondition ByBusinessId(string businessId) {
        return new ScanCondition(nameof(BusinessId), ScanOperator.Equal, businessId);
    }
    
    public static ScanCondition BySKU(string sku) {
        return new ScanCondition(nameof(SKU), ScanOperator.Equal, sku);
    }
}