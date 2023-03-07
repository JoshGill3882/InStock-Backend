using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class BusinessDto : DataTransferObjectSuperType {
    public string BusinessId { get; }
    public string BusinessName { get; }
    public string BusinessOwnerId { get; }

    public BusinessDto(ErrorNotification errorNotes) : base(errorNotes) {}

    public BusinessDto(string businessId, string businessName, string businessOwnerId) {
        BusinessId = businessId;
        BusinessName = businessName;
        BusinessOwnerId = businessOwnerId;
    }
}