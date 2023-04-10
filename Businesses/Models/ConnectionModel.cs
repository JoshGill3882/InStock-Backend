using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace instock_server_application.Businesses.Models;

[DynamoDBTable("Businesses")]
public class ConnectionModel {
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }

    [DynamoDBProperty(typeof(ConnectionObjectConverter))]
    public List<ConnectionObject> Connections { get; set; }
    
    
    public ConnectionModel() {
    }
    
    public ConnectionModel(string businessId) {
        BusinessId = businessId;
        Connections = new List<ConnectionObject>();
    }
    
    public void AddConnectionDetails(string shopName, string authenticationToken)
    {
        Connections ??= new List<ConnectionObject>();
        Connections.Add(new ConnectionObject(shopName, authenticationToken));
    }

    
    public class ConnectionObject {
        public string ShopName { get; set; }

        public string AuthenticationToken { get; set; }
        
        public ConnectionObject() { }

        public ConnectionObject(string shopName, string authenticationToken) {
            this.ShopName = shopName;
            this.AuthenticationToken = authenticationToken;
        }
        
    }

    public class ConnectionObjectConverter : IPropertyConverter {
        public DynamoDBEntry ToEntry(object value) {
            List<ConnectionObject> connections = value as List<ConnectionObject> ?? throw new ArgumentOutOfRangeException();

            string jsonData = JsonSerializer.Serialize(connections);

            DynamoDBEntry entry = new Primitive {
                Value = jsonData
            };
            return entry;
        }

        public object FromEntry(DynamoDBEntry entry) {
            var primitive = entry as Primitive;

            if (primitive == null || !(primitive.Value is string) || string.IsNullOrEmpty((string)primitive.Value))
                throw new ArgumentOutOfRangeException();

            var jsonData = (string)primitive.Value;

            var complexData = JsonSerializer.Deserialize<List<ConnectionObject>>(jsonData) ??
                              throw new ArgumentOutOfRangeException();

            return complexData;
        }
    }
}