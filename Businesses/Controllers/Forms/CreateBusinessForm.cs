namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateBusinessForm {
    public string BusinessName { get; }
    public string BusinessDescription { get; }
    public IFormFile? File { get; }
    
    public CreateBusinessForm(string businessName, string businessDescription, IFormFile? file) {
        BusinessName = businessName;
        BusinessDescription = businessDescription;
        File = file;
    }
}