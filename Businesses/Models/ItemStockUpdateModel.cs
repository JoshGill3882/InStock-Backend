using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Items")]
public abstract class ItemStockUpdateModel {
    [DynamoDBHashKey]
    [DynamoDBProperty("SKU")]
    public string SKU { get; set; }
        
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }

    [DynamoDBProperty("StockUpdates")]
    public List<StockUpdateObject> StockUpdates { get; set; }

    
    public ItemStockUpdateModel() {
    }

    public ItemStockUpdateModel(string sku, string businessId, List<StockUpdateObject> stockUpdates) {
        SKU = sku;
        BusinessId = businessId;
        StockUpdates = stockUpdates;
    }

    public abstract class StockUpdateObject {
        [DynamoDBProperty("StockAmountChanged")]
        public int AmountChanged { get; }
        
        [DynamoDBProperty("ReasonForStockChange")]
        public int ReasonForChange { get; }
        
        public StockUpdateObject(int amountChanged, int reasonForChange) {
            AmountChanged = amountChanged;
            ReasonForChange = reasonForChange;
        }
    }
}