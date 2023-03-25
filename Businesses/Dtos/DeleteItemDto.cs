using System.Security.Claims;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class DeleteItemDto : DataTransferObjectSuperType{
    public string ItemId { get; }
    public string UserBusinessId { get; }
    public string BusinessId { get; }

    public const string USER_UNAUTHORISED_ERROR = "User not authorized for given business";

    public DeleteItemDto(string itemId, string userBusinessId, string businessId) {
        ItemId = itemId;
        UserBusinessId = userBusinessId;
        BusinessId = businessId;
    }

    public DeleteItemDto(ErrorNotification errorNotification) : base(errorNotification) { }
}