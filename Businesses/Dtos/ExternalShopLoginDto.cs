namespace instock_server_application.Businesses.Dtos; 

public class ExternalShopLoginDto {
    
    public string ShopUsername { get; set; }
    
    public string ShopUserPassword { get; set; }

    public ExternalShopLoginDto(string shopUsername, string shopUserPassword) {
        ShopUsername = shopUsername;
        ShopUserPassword = shopUserPassword;
    }
}