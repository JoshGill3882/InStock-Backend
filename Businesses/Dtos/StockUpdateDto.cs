using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class StockUpdateDto : DataTransferObjectSuperType {
    public int ChangeStockAmountBy { get; }
    public string ReasonForChange { get; }
    
    public StockUpdateDto(ErrorNotification errorNotes) : base(errorNotes) {}

    public StockUpdateDto(int changeStockAmountBy, string reasonForChange) {
        ChangeStockAmountBy = changeStockAmountBy;
        ReasonForChange = reasonForChange;
    }
}