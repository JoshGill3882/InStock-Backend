using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Businesses.Models; 

public class Item {
    [DynamoDBHashKey]
    private string SKU { get; set; }
    private string BusinessId { get; set; }
    private string Category { get; set; }
    private string Name { get; set; }
    private int Stock { get; set; }
    private string TableName = "Items";

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
}