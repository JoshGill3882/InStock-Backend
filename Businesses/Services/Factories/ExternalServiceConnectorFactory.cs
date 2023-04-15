using System.Runtime.CompilerServices;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Services.Interfaces;

namespace instock_server_application.Businesses.Services.Abstractions; 

public class ExternalServiceConnectorFactory {
    
    public const string PLATFORM_MOCK_ETSY = "mock etsy"; 
    public const string PLATFORM_MOCK_SHOPIFY = "mock shopify"; 

    public ExternalServiceConnectorFactory() { }
    
    public static bool ValidatePlatformName(string platformName) {
        return platformName.ToLower() == PLATFORM_MOCK_ETSY || platformName.ToLower() == PLATFORM_MOCK_SHOPIFY;
    }
    
    public ExternalShopAuthenticator CreateAuthenticator(
        CreateConnectionForm connectionRequestDetails)
    {
        switch (connectionRequestDetails.PlatformNameConnectingTo.ToLower())
        {
            case PLATFORM_MOCK_ETSY:
                return new MockShopAuthenticator(connectionRequestDetails.ShopUsername, connectionRequestDetails.ShopUserPassword);
            case PLATFORM_MOCK_SHOPIFY:
                return new MockMarketAuthenticator(connectionRequestDetails.ShopUsername,
                    connectionRequestDetails.ShopUserPassword);
            default:
                throw new ArgumentException($"Shop '{connectionRequestDetails.PlatformNameConnectingTo}' is not supported.");
        }
    }
}

