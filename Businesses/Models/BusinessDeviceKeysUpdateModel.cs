using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Businesses.Models; 

public class BusinessDeviceKeysUpdateModel {
    [DynamoDBHashKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }
    
    [DynamoDBProperty("DeviceKeys")]
    public List<string> DeviceKeys { get; set; }

    public BusinessDeviceKeysUpdateModel(string businessId, List<string> deviceKeys) {
        BusinessId = businessId;
        DeviceKeys = deviceKeys;
    }
}