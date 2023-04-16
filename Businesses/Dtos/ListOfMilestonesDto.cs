using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ListOfMilestonesDto : DataTransferObjectSuperType {

    public List<MilestoneDto> ListOfMilestones;
    
    public static string ERROR_UNAUTHORISED = "Unauthorised";

    public ListOfMilestonesDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ListOfMilestonesDto(List<MilestoneDto> listOfMilestones) {
        ListOfMilestones = listOfMilestones;
    }

    public List<Dictionary<string, object>> ToDictionaryForHttpResponse() {
        var listOfMilestones = new List<Dictionary<string, object>>();
        
        foreach (MilestoneDto milestone in ListOfMilestones) {
            listOfMilestones.Add(
                new Dictionary<string, object>(){
                    { nameof(MilestoneDto.MilestoneId), milestone.MilestoneId },
                    { nameof(MilestoneDto.BusinessId), milestone.BusinessId },
                    { nameof(MilestoneDto.ItemSku), milestone.ItemSku },
                    { nameof(MilestoneDto.ItemName), milestone.ItemName },
                    { nameof(MilestoneDto.ImageFilename), milestone.ImageFilename },
                    { nameof(MilestoneDto.TotalSales), milestone.TotalSales },
                    { nameof(MilestoneDto.DateTime), milestone.DateTime },
                    { nameof(MilestoneDto.DisplayMilestone), milestone.DisplayMilestone },
                });
        }

        return listOfMilestones;
    }
}