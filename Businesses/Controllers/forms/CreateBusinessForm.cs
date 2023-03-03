namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateBusinessForm {
    public string BusinessName { get; }

    public CreateBusinessForm(string businessName) {
        BusinessName = businessName;
    }
}