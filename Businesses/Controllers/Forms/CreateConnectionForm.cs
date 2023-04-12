namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateConnectionForm {
    
    public string PlatformNameConnectingTo { get; set; }
    
    public string ShopUsername { get; set; }
    
    public string ShopUserPassword { get; set; }

    public CreateConnectionForm(string platformName, string shopUsername, string shopUserPassword) {
        PlatformNameConnectingTo = platformName;
        ShopUsername = shopUsername;
        ShopUserPassword = shopUserPassword;
    }

    public CreateConnectionForm() {
    }
}