using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

    public void AddStockUpdateDetails(int changeStockAmount, string reasonForChange, DateTime dateTimeAdded) {
        StockUpdates ??= new List<StockUpdateObject>();
        StockUpdates.Add(new StockUpdateObject(changeStockAmount, reasonForChange, dateTimeAdded));
    }
    
    public class StockUpdateObject {
        public int AmountChanged { get; set; }
        public string ReasonForChange { get; set; }
        public DateTime DateTimeAdded { get; set; }

        public StockUpdateObject() {
        }

        public StockUpdateObject(int amountChanged, string reasonForChange, DateTime dateTimeAdded) {
            AmountChanged = amountChanged;
            ReasonForChange = reasonForChange;
            DateTimeAdded = dateTimeAdded;
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