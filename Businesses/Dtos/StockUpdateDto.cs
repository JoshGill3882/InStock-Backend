using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class StockUpdateDto : DataTransferObjectSuperType {
    public int ChangeStockAmountBy { get; }
    public string ReasonForChange { get; }
    public DateTime DateTimeAdded { get; }

    
    public StockUpdateDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public StockUpdateDto(int changeStockAmountBy, string reasonForChange, DateTime dateTimeAdded) {
        ChangeStockAmountBy = changeStockAmountBy;
        ReasonForChange = reasonForChange;
        DateTimeAdded = dateTimeAdded;
    }
}