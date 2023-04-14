using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace instock_server_application.Businesses.Dtos; 

public class CreateItemOrderRequestDto : DataTransferObjectSuperType {

    public string UserId { get; }
    public string UserBusinessId { get; }
    
    public string BusinessId { get; }
    public string ItemSku { get; }
    public int AmountOrdered { get; }
   
    public CreateItemOrderRequestDto(string userId, string userBusinessId, string businessId, string itemSku, int amountOrdered) {
        UserId = userId;
        UserBusinessId = userBusinessId;
        BusinessId = businessId;
        ItemSku = itemSku;
        AmountOrdered = amountOrdered;
    }
}