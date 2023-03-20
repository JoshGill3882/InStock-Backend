using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Items")]
public class ItemStockUpdateModel {
    [DynamoDBHashKey]
    [DynamoDBProperty("SKU")]
    public string SKU { get; set; }
        
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }

    [DynamoDBProperty(typeof(StockUpdateObjectConverter))]
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
        public int AmountChanged { get; }
        
        public string ReasonForChange { get; }

        public StockUpdateObject() {
        }

        public StockUpdateObject(int amountChanged, string reasonForChange) {
            AmountChanged = amountChanged;
            ReasonForChange = reasonForChange;
        }
    }
    public class StockUpdateObjectConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            List<StockUpdateObject> stockUpdate = value as List<StockUpdateObject> ?? throw new ArgumentOutOfRangeException();

            string jsonData = JsonSerializer.Serialize(stockUpdate);

            DynamoDBEntry entry = new Primitive
            {
                Value = jsonData
            };
            return entry;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            Primitive? primitive = entry as Primitive;
            
            if (primitive == null || !(primitive.Value is String) || string.IsNullOrEmpty((string)primitive.Value))
                throw new ArgumentOutOfRangeException();

            string jsonData = (string) primitive.Value;
            
            List<StockUpdateObject> complexData = JsonSerializer.Deserialize<List<StockUpdateObject>>(jsonData) ?? throw new ArgumentOutOfRangeException();
            
            return complexData;
        }
    }
}