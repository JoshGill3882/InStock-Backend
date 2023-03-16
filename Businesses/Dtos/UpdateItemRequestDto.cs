using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Controllers.forms;
using Microsoft.AspNetCore.JsonPatch;

namespace instock_server_application.Businesses.Dtos; 

public class UpdateItemRequestDto {
    public string UserId { get; }
    public string UserBusinessId { get; }
    public string ItemId { get; }
    public JsonPatchDocument ItemPatchDocument { get; }
    
    public UpdateItemRequestDto(string userId, string userBusinessId, JsonPatchDocument itemPatchDocument, string itemId) {
        UserId = userId;
        UserBusinessId = userBusinessId;
        ItemPatchDocument = itemPatchDocument;
        ItemId = itemId;
    }
}