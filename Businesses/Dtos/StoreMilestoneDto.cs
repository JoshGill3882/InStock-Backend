namespace instock_server_application.Businesses.Dtos; 

public class StoreMilestoneDto {
    public string MilestoneId { get; set; }
    public string BusinessId { get; set; }
    public string ItemSku { get; set; }
    public string ItemName { get; set; }
    public int TotalSales { get; set; }
    public long DateTime { get; set; }
    public bool DisplayMilestone { get; set; }

    public StoreMilestoneDto(string milestoneId, string businessId, string itemSku, string itemName, int totalSales, long dateTime, bool displayMilestone) {
        MilestoneId = milestoneId;
        BusinessId = businessId;
        ItemSku = itemSku;
        ItemName = itemName;
        TotalSales = totalSales;
        DateTime = dateTime;
        DisplayMilestone = displayMilestone;
    }
}