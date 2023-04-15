using System.Runtime.CompilerServices;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Services.Interfaces;

namespace instock_server_application.Businesses.Services.Abstractions; 

public static class ExternalServiceConnectorFactory {
    
    public const string PLATFORM_MOCK_ETSY = "mock etsy"; 
    public const string PLATFORM_MOCK_SHOPIFY = "mock shopify"; 

    public static bool ValidatePlatformName(string platformName) {
        return platformName.ToLower() == PLATFORM_MOCK_ETSY || platformName.ToLower() == PLATFORM_MOCK_SHOPIFY;
    }
    
    public static ExternalShopConnectorService CreateConnector(
        string platformName)
    {
        switch (platformName.ToLower())
        {
            case PLATFORM_MOCK_ETSY:
                return new MockShopConnectorService();
            case PLATFORM_MOCK_SHOPIFY:
                return new MockMarketConnectorService();
            default:
                throw new ArgumentException($"Shop '{platformName.ToLower()}' is not supported.");
        }
    }
}

