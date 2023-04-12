using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using instock_server_application.Businesses.Dtos;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace instock_server_application.Businesses.Models;

[DynamoDBTable("Businesses")]
public class ConnectionModel {
    [DynamoDBHashKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }

    [DynamoDBProperty(typeof(ConnectionObjectConverter))]
    public List<ConnectionObject>? Connections { get; set; }
    
    
    public ConnectionModel() {
    }
    
    public ConnectionModel(string businessId) {
        BusinessId = businessId;
        Connections = new List<ConnectionObject>();
    }
    
    public void AddConnectionDetails(string shopName, string authenticationToken, string shopUsername)
    {
        Connections ??= new List<ConnectionObject>();
        Connections.Add(new ConnectionObject(shopName, authenticationToken, shopUsername));
    }

    public override string ToString() {
        return $"ConnectionModel{{ BusinessId='{BusinessId}', Connections='{JsonSerializer.Serialize(Connections)}' }}";
    }
    
    public StoreConnectionDto ToStoreConnectionDto() {
        List<ConnectionDto> connectionDtos = new List<ConnectionDto>();

        // By default businesses have null connections
        if (Connections != null) {
            foreach (ConnectionObject connection in Connections) {
                connectionDtos.Add(new ConnectionDto(
                    platformName: connection.PlatformName, 
                    authenticationToken: connection.AuthenticationToken, 
                    shopUsername: connection.ShopUsername));
            }
        }

        return new StoreConnectionDto(BusinessId, connectionDtos);
    }

    
    public class ConnectionObject {
        public string PlatformName { get; set; }

        public string AuthenticationToken { get; set; }

        public string ShopUsername { get; set; }
        public ConnectionObject() { }

        public ConnectionObject(string platformName, string authenticationToken, string shopUsername) {
            this.PlatformName = platformName;
            this.AuthenticationToken = authenticationToken;
            this.ShopUsername = shopUsername; 
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
