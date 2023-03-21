namespace instock_server_application.Businesses.Controllers.forms;

public class CreateStockUpdateForm {
    public string ChangeStockAmountBy { get; }
    public string ReasonForChange { get; }
    
    public CreateStockUpdateForm(string changeStockAmountBy, string reasonForChange) {
        ChangeStockAmountBy = changeStockAmountBy;
        ReasonForChange = reasonForChange;
    }
}