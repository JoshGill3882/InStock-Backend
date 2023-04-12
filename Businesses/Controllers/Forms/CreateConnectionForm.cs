namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateConnectionForm {
    
    public string ShopNameConnectingTo { get; set; }
    
    public string ShopUsername { get; set; }
    
    public string ShopUserPassword { get; set; }

    public CreateConnectionForm(string shopName, string shopUsername, string shopUserPassword) {
        ShopNameConnectingTo = shopName;
        ShopUsername = shopUsername;
        ShopUserPassword = shopUserPassword;
    }

    public CreateConnectionForm() {
    }
}