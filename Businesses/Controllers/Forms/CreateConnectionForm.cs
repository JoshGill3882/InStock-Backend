namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateConnectionForm {
    
    public string PlatformNameConnectingTo { get; set; }
    
    public string ShopUsername { get; set; }
    
    public string ShopUserPassword { get; set; }

    public CreateConnectionForm(string platformNameConnectingTo, string shopUsername, string shopUserPassword) {
        PlatformNameConnectingTo = platformNameConnectingTo;
        ShopUsername = shopUsername;
        ShopUserPassword = shopUserPassword;
    }

    public CreateConnectionForm() {
    }
}