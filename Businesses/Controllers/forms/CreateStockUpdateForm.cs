namespace instock_server_application.Businesses.Controllers.forms;

public class CreateStockUpdateForm {
    public int ChangeStockAmountBy { get; }
    public string ReasonForChange { get; }
    
    public CreateStockUpdateForm(int changeStockAmountBy, string reasonForChange) {
        ChangeStockAmountBy = changeStockAmountBy;
        ReasonForChange = reasonForChange;
    }
}