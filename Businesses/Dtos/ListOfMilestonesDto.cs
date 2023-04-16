using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ListOfMilestonesDto : DataTransferObjectSuperType {

    public List<StoreMilestoneDto> ListOfMilestones;
    
    public static string ERROR_UNAUTHORISED = "Unauthorised";

    public ListOfMilestonesDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ListOfMilestonesDto(List<StoreMilestoneDto> listOfMilestones) {
        ListOfMilestones = listOfMilestones;
    }

    public List<Dictionary<string, object>> ToDictionaryForHttpResponse() {
        var listOfMilestones = new List<Dictionary<string, object>>();
        
        foreach (StoreMilestoneDto milestone in ListOfMilestones) {
            listOfMilestones.Add(
                new Dictionary<string, object>(){
                    { nameof(StoreMilestoneDto.MilestoneId), milestone.MilestoneId },
                    { nameof(StoreMilestoneDto.BusinessId), milestone.BusinessId },
                    { nameof(StoreMilestoneDto.ItemSku), milestone.ItemSku },
                    { nameof(StoreMilestoneDto.ItemName), milestone.ItemName },
                    { nameof(StoreMilestoneDto.TotalSales), milestone.TotalSales },
                    { nameof(StoreMilestoneDto.DateTime), milestone.DateTime },
                    { nameof(StoreMilestoneDto.DisplayMilestone), milestone.DisplayMilestone },
                });
        }

        return listOfMilestones;
    }
}