using System.Security.Claims;

namespace instock_server_application.Businesses.Dtos; 

public class DeleteItemDto {
    public ClaimsPrincipal User { get; }
    public string ItemId { get; }
    public string BusinessId { get; }

    public DeleteItemDto(ClaimsPrincipal user, string itemId, string businessId) {
        User = user;
        ItemId = itemId;
        BusinessId = businessId;
    }
}