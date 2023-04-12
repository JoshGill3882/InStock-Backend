using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Services.Interfaces;

namespace instock_server_application.Businesses.Services.Abstractions; 

public class ExternalServiceConnectorFactory {
    public ExternalServiceConnectorFactory() { }

    public ExternalShopAuthenticator CreateAuthenticator(
        CreateConnectionForm connectionRequestDetails)
    {
        switch (connectionRequestDetails.PlatformNameConnectingTo.ToLower())
        {
            case "mockshop":
                return new MockShopAuthenticator(connectionRequestDetails.ShopUsername, connectionRequestDetails.ShopUserPassword);
            case "mockmarket":
                return new MockMarketAuthenticator(connectionRequestDetails.ShopUsername,
                    connectionRequestDetails.ShopUserPassword);
            default:
                throw new ArgumentException($"Shop '{connectionRequestDetails.PlatformNameConnectingTo}' is not supported.");
        }
    }
}

