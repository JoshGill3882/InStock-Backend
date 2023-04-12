using System.Text;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Exceptions;
using instock_server_application.Businesses.Services.Interfaces;
using Newtonsoft.Json;

namespace instock_server_application.Businesses.Services; 

public class MockShopAuthenticator : ExternalShopAuthenticator {
    
    private String Username;

    private String Password;

    public MockShopAuthenticator(string username, string password) : base(username, password) {
        Username = username;
        Password = password;
    }
    
    
    public override async Task<ExternalShopAuthenticationTokenDto> LoginToShop(ExternalShopLoginDto loginDetails) {
        HttpClient httpClient = new HttpClient(); 
        
        var uri = new Uri("http://instock.eu.pythonanywhere.com/login");
        
        var loginData = new Dictionary<string, string>
        {
            { "shopName", Username },
            { "password", Password }
        };

        var json = JsonConvert.SerializeObject(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(uri, content);
        
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


}