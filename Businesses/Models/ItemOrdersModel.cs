using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Items")]
public class ItemOrdersModel {
    
    private int _totalOrders;

    [DynamoDBHashKey]
    [DynamoDBProperty("SKU")]
    public string SKU { get; set; }
        
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }
    
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
    
    [DynamoDBProperty(typeof(ItemOrderObjectConverter))]
    public List<ItemOrderObject> ItemOrders { get; set; }

    
    public ItemOrdersModel() {
    }

    public ItemOrdersModel(string sku, string businessId, int totalOrders) {
        SKU = sku;
        BusinessId = businessId;
        _totalOrders = totalOrders;
        ItemOrders = new List<ItemOrderObject>();
    }
        
    public int GetTotalOrders() {
        return _totalOrders;
    }

    public void AddItemOrderDetails(int amountOrdered, DateTime dateTimeAdded) {
        ItemOrders ??= new List<ItemOrderObject>();
        ItemOrders.Add(new ItemOrderObject(amountOrdered, dateTimeAdded));
        _totalOrders += amountOrdered;
    }
    
    public class ItemOrderObject {
        public int AmountOrdered { get; set; }
        public DateTime DateTimeAdded { get; set; }

        public ItemOrderObject() {
        }

        public ItemOrderObject(int amountOrdered, DateTime dateTimeAdded) {
            AmountOrdered = amountOrdered;
            DateTimeAdded = dateTimeAdded;
        }
    }
    public class ItemOrderObjectConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            List<ItemOrderObject> itemOrder = value as List<ItemOrderObject> ?? throw new ArgumentOutOfRangeException();

            string jsonData = JsonSerializer.Serialize(itemOrder);

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
            
            List<ItemOrderObject> complexData = JsonSerializer.Deserialize<List<ItemOrderObject>>(jsonData) ?? throw new ArgumentOutOfRangeException();
            
            return complexData;
        }
    }
}