using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class ExternalShopAuthenticationTokenDto : DataTransferObjectSuperType {
    
    public string AuthenticationToken { get; }
    
    public ExternalShopAuthenticationTokenDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ExternalShopAuthenticationTokenDto(string authenticationToken) {
        AuthenticationToken = authenticationToken;
    }
}