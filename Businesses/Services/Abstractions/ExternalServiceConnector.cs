using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Services.Interfaces; 

public abstract class ExternalShopAuthenticator {
    
    public abstract string? AuthorisationToken { get; protected internal set; }

    public ExternalShopAuthenticator() {}

    public ExternalShopAuthenticator(string? authorisationToken) {
        AuthorisationToken = authorisationToken;
    }

    public bool IsAuthenticated() {
        return AuthorisationToken != null;
    }
    
    public abstract Task<ExternalShopAuthenticationTokenDto> LoginToShop(ExternalShopLoginDto loginDetails);
    
    //contains case statements 
}