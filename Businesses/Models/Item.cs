using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace instock_server_application.Businesses.Models;

[DynamoDBTable("Items")]
public class Item {
    // The value for the items stock, this is so we get more control over the value
    private int _totalStock;
    private int _totalOrders;

    
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
    
    [DynamoDBProperty("ImageFilename")]
    public string? ImageFilename { get; set; }

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
    
    public Item() {}

    /// <summary>
    /// All Args Constructor
    /// </summary>
    /// <param name="sku"> Item's Unique Key </param>
    /// <param name="businessId"> Item's Business Id </param>
    /// <param name="category"> Item's Category </param>
    /// <param name="name"> Item's Name </param>
    /// <param name="stock">Item's Stock Level</param>
    /// <param name="imageUrl">Item's Image</param>
    public Item(string sku, string businessId, string category, string name, int totalStock, int totalOrders, string? imageFilename) {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        _totalStock = totalStock;
        _totalOrders = totalStock;
        ImageFilename = imageFilename;
    }

    public Item(string sku, string businessId) {
        SKU = sku;
        BusinessId = businessId;
    }

    public int GetTotalStock() {
        return _totalStock;
    }
    
    public int GetTotalOrders() {
        return _totalOrders;
    }
    
    // Scan conditions used in scans for this model
    public static ScanCondition ByBusinessId(string businessId) {
        return new ScanCondition(nameof(BusinessId), ScanOperator.Equal, businessId);
    }
    public static ScanCondition ByBusinessName(string businessName) {
        return new ScanCondition(nameof(Name), ScanOperator.Equal, businessName);
    }
    public static ScanCondition ByBusinessSku(string businessSku) {
        return new ScanCondition(nameof(SKU), ScanOperator.Equal, businessSku);
    }
}