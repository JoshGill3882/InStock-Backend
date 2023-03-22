using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Businesses")]
public class BusinessModel {
    public static readonly string TableName = "Businesses";
    
    [DynamoDBHashKey]
    [DynamoDBProperty("BusinessId")]
    public String BusinessId { get; set; }

    [DynamoDBProperty("Name")] public string BusinessName { get; set; }

    [DynamoDBProperty("Description")]
    public string BusinessDescription { get; set; }
    
    [DynamoDBProperty("Owner")] 
    public String OwnerId { get; set; }

    public BusinessModel() {
    }

    public BusinessModel(string id, string name, string owner, string businessDescription) {
        BusinessId = id;
        BusinessName = name;
        BusinessDescription = businessDescription;
        OwnerId = owner;
    }
    
    public BusinessModel(Guid id, string name, string owner, string businessDescription) {
        BusinessId = id.ToString();
        BusinessName = name;
        BusinessDescription = businessDescription;
        OwnerId = new Guid(owner).ToString();
    }
    
    public BusinessModel(Guid id, string name, Guid owner, string businessDescription) {
        BusinessId = id.ToString();
        BusinessName = name;
        OwnerId = owner.ToString();
        BusinessDescription = businessDescription;
    }
}