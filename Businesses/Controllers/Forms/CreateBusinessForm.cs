namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateBusinessForm {
    public string BusinessName { get; set; }
    public string BusinessDescription { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string DeviceKey { get; set; }

    public CreateBusinessForm(string businessName, string businessDescription, IFormFile? imageFile, string deviceKey) {
        BusinessName = businessName;
        BusinessDescription = businessDescription;
        ImageFile = imageFile;
        DeviceKey = deviceKey;
    }

    public CreateBusinessForm() {
        // Parameterless constructor required for model binding
    }
}