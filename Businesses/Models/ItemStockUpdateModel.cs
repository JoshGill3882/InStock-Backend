using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Items")]
public class ItemStockUpdateModel {
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

    public ItemStockUpdateModel(string sku, string businessId) {
        SKU = sku;
        BusinessId = businessId;
        StockUpdates = new List<StockUpdateObject>() ;
    }

    public void AddStockUpdateDetails(int changeStockAmount, string reasonForChange) {
        StockUpdates.Add(new StockUpdateObject(changeStockAmount, reasonForChange));
    }
    
    public class StockUpdateObject {
        [DynamoDBProperty("StockAmountChanged")]
        public int AmountChanged { get; }
        
        [DynamoDBProperty("ReasonForStockChange")]
        public string ReasonForChange { get; }

        public StockUpdateObject() {
        }

        public StockUpdateObject(int amountChanged, string reasonForChange) {
            AmountChanged = amountChanged;
            ReasonForChange = reasonForChange;
        }
    }
}