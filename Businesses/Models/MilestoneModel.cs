using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using instock_server_application.Businesses.Dtos;

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
    
    [DynamoDBProperty("ImageFilename")]
    public string? ImageFilename { get; set; }
    
    [DynamoDBProperty("TotalSales")]
    public int TotalSales { get; set; }
    
    [DynamoDBProperty("DateTimeAchieved")]
    public long DateTime { get; set; }
    
    [DynamoDBProperty("DisplayMilestone")]
    public bool DisplayMilestone { get; set; }

    public MilestoneModel() {
    }

    public MilestoneModel(string milestoneId, string businessId, string itemSku, string itemName, string? imageFilename, int totalSales, long dateTime, bool displayMilestone) {
        MilestoneId = milestoneId;
        BusinessId = businessId;
        ItemSku = itemSku;
        ItemName = itemName;
        ImageFilename = imageFilename;
        TotalSales = totalSales;
        DateTime = dateTime;
        DisplayMilestone = displayMilestone;
    }

    public MilestoneModel(MilestoneDto milestoneDto) {
        MilestoneId = milestoneDto.MilestoneId;
        BusinessId = milestoneDto.BusinessId;
        ItemSku = milestoneDto.ItemSku;
        ItemName = milestoneDto.ItemName;
        ImageFilename = milestoneDto.ImageFilename;
        TotalSales = milestoneDto.TotalSales;
        DateTime = milestoneDto.DateTime;
        DisplayMilestone = milestoneDto.DisplayMilestone;
    }
    
    public static ScanCondition ByBusinessId(string businessId) {
        return new ScanCondition(nameof(BusinessId), ScanOperator.Equal, businessId);
    }
    
    public static ScanCondition WhereDisplayMilestoneTrue() {
        return new ScanCondition(nameof(DisplayMilestone), ScanOperator.Equal, true);
    }
}