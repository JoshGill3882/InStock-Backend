using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class CategoryDto : DataTransferObjectSuperType {
    public string UserBusinessId { get; }
    public string BusinessId { get; }

    public CategoryDto(string userBusinessId, string businessId) {
        UserBusinessId = userBusinessId;
        BusinessId = businessId;
    }

    public CategoryDto(ErrorNotification errorNotification) : base(errorNotification) { }
}