using System.Numerics;
using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Businesses.Models; 
[DynamoDBTable("Items")]
public class Item {
    public static readonly string TableName = "Items";

    // The value for the items stock, this is so we get more control over the value
    private int _stock;
    
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
    public String Stock {
        get => _stock.ToString();
        set {
            
            
            if (value.Length > int.MaxValue.ToString().Length) {
                if (value.Contains('-')) {
                    _stock = int.MinValue;
                }
                else {
                    _stock = int.MaxValue;
                }
            } else if ( long.Parse(value) >= int.MaxValue) {
                _stock = int.MaxValue;
            } else if (long.Parse(value) <= int.MinValue) {
                _stock = int.MinValue;
            }
            else {
                _stock = int.Parse(value);
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
    public Item(string sku, string businessId, string category, string name, int stock) {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        _stock = stock;
    }

    public Item(string sku, string businessId) {
        SKU = sku;
        BusinessId = businessId;
    }

    public int GetStock() {
        return _stock;
    }
}