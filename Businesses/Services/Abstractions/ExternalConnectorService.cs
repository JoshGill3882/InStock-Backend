using System.Text;
using instock_server_application.Businesses.Dtos;
using Newtonsoft.Json;

namespace instock_server_application.Businesses.Services.Interfaces; 

public abstract class ExternalShopConnectorService {
    
    public abstract string? AuthorisationToken { get; protected internal set; }

    public ExternalShopConnectorService() {}

    public ExternalShopConnectorService(string? authorisationToken) {
        AuthorisationToken = authorisationToken;
    }

    public bool IsAuthenticated() {
        return AuthorisationToken != null;
    }

    public static Task<HttpResponseMessage> PostJsonRequest(string uri, string json) {
        HttpClient httpClient = new HttpClient();
        var uriAddress = new Uri(uri);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return httpClient.PostAsync(uriAddress, content);
    }
    
    public static Task<HttpResponseMessage> PutJsonRequest(string uri, string json) {
        HttpClient httpClient = new HttpClient();
        var uriAddress = new Uri(uri);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return httpClient.PutAsync(uriAddress, content);
    }

    public static Task<HttpResponseMessage> GetRequest(string uri) {
        HttpClient httpClient = new HttpClient();
        var uriAddress = new Uri(uri);
        return httpClient.GetAsync(uriAddress);
    }

    public abstract Task<ExternalShopAuthenticationTokenDto> LoginToShop(ExternalShopLoginDto loginDetails);

    public abstract Task<bool> HasItemSku(string platformUsername, string platformItemSku);

    public abstract Task<ConnectedItemDetailsDto> GetConnectedItemDetails(string itemSku);

    public abstract void SetItemStock(string businessId, string itemSku, int totalStock);
    
    public abstract string GetPlatformImageUrl();
}