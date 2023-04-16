using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class HideMilestoneDto : DataTransferObjectSuperType {
    public string MilestoneId { get; }
    public string BusinessId { get; }

    public HideMilestoneDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public HideMilestoneDto(string milestoneId, string businessId) {
        MilestoneId = milestoneId;
        BusinessId = businessId;
    }
}