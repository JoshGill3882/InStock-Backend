using System.Security.Claims;

namespace instock_server_application.Businesses.Dtos; 

public class DeleteItemDto {
    public string ItemId { get; }
    public string BusinessId { get; }

    public DeleteItemDto(string itemId, string businessId) {
        ItemId = itemId;
        BusinessId = businessId;
    }
}