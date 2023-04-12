using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Businesses")]
public class BusinessModel {
    public static readonly string TableName = "Businesses";
    
    [DynamoDBHashKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }

    [DynamoDBProperty("Name")] public string BusinessName { get; set; }

    [DynamoDBProperty("Description")]
    public string BusinessDescription { get; set; }
    
    [DynamoDBProperty("Owner")] 
    public string OwnerId { get; set; }
    
    [DynamoDBProperty("DeviceKeys")]
    public List<string> DeviceKeys { get; set; }
    [DynamoDBProperty("ImageFilename")]
    public string? ImageFilename { get; set; }

    public BusinessModel() {
    }

    public BusinessModel(string id, string name, string owner, string businessDescription, string? imageFilename, List<string> deviceKeys) {
        BusinessId = id;
        BusinessName = name;
        BusinessDescription = businessDescription;
        OwnerId = owner;
        ImageFilename = imageFilename;
        DeviceKeys = deviceKeys;
    }
    
    public BusinessModel(Guid id, string name, string owner, string businessDescription, string? imageFilename, List<string> deviceKeys) {
        BusinessId = id.ToString();
        BusinessName = name;
        BusinessDescription = businessDescription;
        OwnerId = new Guid(owner).ToString();
        DeviceKeys = deviceKeys;
        ImageFilename = imageFilename;
    }
    
    public BusinessModel(Guid id, string name, Guid owner, string businessDescription, string? imageFilename, List<string> deviceKeys) {
        BusinessId = id.ToString();
        BusinessName = name;
        OwnerId = owner.ToString();
        BusinessDescription = businessDescription;
        ImageFilename = imageFilename;
        DeviceKeys = deviceKeys;
    }
}