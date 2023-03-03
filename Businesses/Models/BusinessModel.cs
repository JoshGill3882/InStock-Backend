using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Businesses")]
public class BusinessModel {
    [DynamoDBHashKey]
    [DynamoDBProperty("BusinessId")]
    public Guid BusinessId { get; set; }

    [DynamoDBProperty("Name")]
    public string BusinessName { get; set; }
    
    [DynamoDBProperty("Owner")] 
    public string OwnerId { get; set; }

    public BusinessModel() {
    }

    public BusinessModel(string id, string name, string owner) {
        BusinessId = new Guid(id);
        BusinessName = name;
        OwnerId = owner;
    }
    
    public BusinessModel(Guid id, string name, string owner) {
        BusinessId = id;
        BusinessName = name;
        OwnerId = owner;
    }
}