namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateBusinessForm {
    public string BusinessName { get; }
    public string BusinessDescription { get; }
    public CreateBusinessForm(string businessName, string businessDescription) {
        BusinessName = businessName;
        BusinessDescription = businessDescription;
    }
}