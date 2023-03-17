namespace instock_server_application.Businesses.Dtos; 

public class StoreStockUpdateDto {

    public string BusinessId { get; }
    public string ItemId { get; }
    public int ChangeStockAmountBy { get; }
    public string ReasonForChange { get; }
    
    public StoreStockUpdateDto(string businessId, string itemId, int changeStockAmountBy, string reasonForChange) {
        BusinessId = businessId;
        ItemId = itemId;
        ChangeStockAmountBy = changeStockAmountBy;
        ReasonForChange = reasonForChange;
    }
}