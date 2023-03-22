using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace instock_server_application.Businesses.Dtos; 

public class CreateStockUpdateRequestDto : DataTransferObjectSuperType {

    public string UserId { get; }
    public string UserBusinessId { get; }
    
    public string BusinessId { get; }
    public string ItemSku { get; }
    public int ChangeStockAmountBy { get; }
    public string ReasonForChange { get; }


    public CreateStockUpdateRequestDto(string userId, string userBusinessId, string businessId, int changeStockAmountBy, string reasonForChange, string itemSku) {
        UserId = userId;
        UserBusinessId = userBusinessId;
        BusinessId = businessId;
        ReasonForChange = reasonForChange;
        ItemSku = itemSku;
        ChangeStockAmountBy = changeStockAmountBy;
    }
}