using instock_server_application.Businesses.Models;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class MilestoneDto : DataTransferObjectSuperType {
    public string MilestoneId { get; set; }
    public string BusinessId { get; set; }
    public string ItemSku { get; set; }
    public string ItemName { get; set; }
    public string? ImageFilename { get; set; }
    public int TotalSales { get; set; }
    public long DateTime { get; set; }
    public bool DisplayMilestone { get; set; }
    
    public MilestoneDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public MilestoneDto(string milestoneId, string businessId, string itemSku, string itemName, string? imageFilename, int totalSales, long dateTime, bool displayMilestone) {
        MilestoneId = milestoneId;
        BusinessId = businessId;
        ItemSku = itemSku;
        ItemName = itemName;
        ImageFilename = imageFilename;
        TotalSales = totalSales;
        DateTime = dateTime;
        DisplayMilestone = displayMilestone;
    }
    
    public MilestoneDto(MilestoneModel milestoneModel) {
        MilestoneId = milestoneModel.MilestoneId;
        BusinessId = milestoneModel.BusinessId;
        ItemSku = milestoneModel.ItemSku;
        ItemName = milestoneModel.ItemName;
        ImageFilename = milestoneModel.ImageFilename;
        TotalSales = milestoneModel.TotalSales;
        DateTime = milestoneModel.DateTime;
        DisplayMilestone = milestoneModel.DisplayMilestone;
    }
}