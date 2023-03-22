namespace instock_server_application.Businesses.Controllers.forms;

public class CreateItemOrderForm {
    public string AmountOrdered { get; }
    
    public CreateItemOrderForm(string amountOrdered) {
        AmountOrdered = amountOrdered;
    }
}