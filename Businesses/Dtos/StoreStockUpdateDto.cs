using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class StoreStockUpdateDto {

    public string BusinessId { get; }
    public string ItemSku { get; }
    public int ChangeStockAmountBy { get; }
    public string ReasonForChange { get; }
    public DateTime DateTimeAdded { get; }


    public StoreStockUpdateDto(string businessId, string itemSku, int changeStockAmountBy, string reasonForChange, DateTime dateTimeAdded) {
        BusinessId = businessId;
        ItemSku = itemSku;
        ChangeStockAmountBy = changeStockAmountBy;
        ReasonForChange = reasonForChange;
        DateTimeAdded = dateTimeAdded;
    }
}