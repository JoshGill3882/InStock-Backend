namespace instock_server_application.Businesses.Controllers.forms;

public class CreateItemConnectionForm {
    public string PlatformName { get; set; }
    public string PlatformItemSku { get; set; }
    
    public CreateItemConnectionForm(string platformName, string platformItemSku) {
        PlatformName = platformName;
        PlatformItemSku = platformItemSku;
    }
}