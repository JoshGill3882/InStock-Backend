namespace instock_server_application.Businesses.Controllers.forms;

public class CreateItemOrderForm {
    public string AmountOrdered { get; set; }

    public CreateItemOrderForm(string amountOrdered) {
        AmountOrdered = amountOrdered;
    }
}