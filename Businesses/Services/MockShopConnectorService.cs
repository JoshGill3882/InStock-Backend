using System.Text;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Exceptions;
using instock_server_application.Businesses.Services.Interfaces;
using Newtonsoft.Json;

namespace instock_server_application.Businesses.Services; 

public class MockShopConnectorService : ExternalShopConnectorService {
    public override string? AuthorisationToken { get; protected internal set; }
    private const string UriAddress = "http://instock.eu.pythonanywhere.com/";

    public override async Task<ExternalShopAuthenticationTokenDto> LoginToShop(ExternalShopLoginDto loginDetails) {
        
        var loginData = new Dictionary<string, string>
        {
            { "username", loginDetails.ShopUsername },
            { "password", loginDetails.ShopUserPassword }
        };

        string uri = UriAddress + "login";
        string json = JsonConvert.SerializeObject(loginData);

        HttpResponseMessage response = await PostJsonRequest(uri, json);
        
        if (response.IsSuccessStatusCode)
        {
            var resultJson = await response.Content.ReadAsStringAsync();
            var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultJson);

            if (jsonDict == null) {
                throw new LoginToExternalShopFailedException("Null value returned from shop");
            }
            
            string authToken = jsonDict["authToken"];
            ExternalShopAuthenticationTokenDto authenticationToken = new ExternalShopAuthenticationTokenDto(authToken);
            
            return authenticationToken;
        }
        else
        {
            throw new LoginToExternalShopFailedException(response.ReasonPhrase!);
        }
    }
    
    public override async Task<bool> HasItemSku(string platformUsername, string platformItemSku) {
        string uri = UriAddress + "listings";

        HttpResponseMessage response = await GetRequest(uri);

        if (!response.IsSuccessStatusCode) throw new ExternalConnectionFailedException("Null value returned from shop");
        
        string resultJson = await response.Content.ReadAsStringAsync();
        List<Dictionary<string, string>>? dictContent =
            JsonConvert.DeserializeObject<List<Dictionary<string, String>>>(resultJson);

        if (dictContent == null) {
            throw new ExternalConnectionFailedException("Null value returned from shop");
        }
            
        foreach (Dictionary<string, string> item in dictContent) {
            if (item.ContainsKey("shopName") && item.ContainsKey("itemId")) {
                if (item["shopName"] == platformUsername && item["itemId"] == platformItemSku) {
                    return true;
                }
            }
        }

        return false;
    }
}