namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateBusinessForm {
    public string BusinessName { get; set; }
    public string BusinessDescription { get; set; }
    public IFormFile? File { get; set; }

    public CreateBusinessForm(string businessName, string businessDescription, IFormFile? file) {
        BusinessName = businessName;
        BusinessDescription = businessDescription;
        File = file;
    }

    public CreateBusinessForm() {
        // Parameterless constructor required for model binding
    }
}