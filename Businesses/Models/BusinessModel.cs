using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Businesses")]
public class BusinessModel {
    [DynamoDBHashKey] 
    public int BusinessId { get; set; }

    [DynamoDBProperty("Name")]
    public string BusinessName { get; set; }
    
    [DynamoDBProperty("Owner")] 
    public string OwnerId { get; set; }
    
    [DynamoDBProperty("Members")]
    public List<string> MembersIdList { get; set; }
    
    public BusinessModel(int id, string name, string owner, List<string> membersList) {
        BusinessId = id;
        BusinessName = name;
        OwnerId = owner;
        MembersIdList = membersList;
    }
}