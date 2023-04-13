namespace instock_server_application.Businesses.Dtos; 

public class ExternalShopLoginDto {
    
    public string ShopUsername { get; }
    
    public string ShopUserPassword { get; }

    public ExternalShopLoginDto(string shopUsername, string shopUserPassword) {
        ShopUsername = shopUsername;
        ShopUserPassword = shopUserPassword;
    }
}