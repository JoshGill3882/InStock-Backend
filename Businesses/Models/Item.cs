using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Businesses.Models; 
[DynamoDBTable("Items")]
public class Item {
    
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
    public int Stock { get; set; }
    
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
        Stock = stock;
    }

    public Item(string sku, string businessId) {
        SKU = sku;
        BusinessId = businessId;
    }
}