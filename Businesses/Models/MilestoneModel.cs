using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace instock_server_application.Businesses.Models; 

[DynamoDBTable("Milestones")]
public class MilestoneModel {
    [DynamoDBHashKey]
    [DynamoDBProperty("MilestoneId")]
    public string MilestoneId { get; set; }
    
    [DynamoDBRangeKey]
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }

    [DynamoDBProperty("ItemSKU")]
    public string ItemSku { get; set; }
    
    [DynamoDBProperty("ItemName")]
    public string ItemName { get; set; }
    
    [DynamoDBProperty("TotalSales")]
    public int TotalSales { get; set; }
    
    [DynamoDBProperty("DateTimeAchieved")]
    public long DateTime { get; set; }
    
    [DynamoDBProperty("DisplayMilestone")]
    public bool DisplayMilestone { get; set; }

    public MilestoneModel() {
    }

    public MilestoneModel(string milestoneId, string businessId, string itemSku, string itemName, int totalSales, long dateTime, bool displayMilestone) {
        MilestoneId = milestoneId;
        BusinessId = businessId;
        ItemSku = itemSku;
        ItemName = itemName;
        TotalSales = totalSales;
        DateTime = dateTime;
        DisplayMilestone = displayMilestone;
    }
    
    public static ScanCondition ByBusinessId(string businessId) {
        return new ScanCondition(nameof(BusinessId), ScanOperator.Equal, businessId);
    }
}