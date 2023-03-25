using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class ValidateBusinessIdDto : DataTransferObjectSuperType {
    public string UserBusinessId { get; }
    public string BusinessId { get; }

    public ValidateBusinessIdDto(string userBusinessId, string businessId) {
        UserBusinessId = userBusinessId;
        BusinessId = businessId;
    }

    public ValidateBusinessIdDto(ErrorNotification errorNotification) : base(errorNotification) { }
}