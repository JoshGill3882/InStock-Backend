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
}