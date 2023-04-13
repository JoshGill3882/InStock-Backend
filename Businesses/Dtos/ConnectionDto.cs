using instock_server_application.Shared.Dto;
using System;

namespace instock_server_application.Businesses.Dtos
{
    public class ConnectionDto : DataTransferObjectSuperType
    {
        public string PlatformName { get; }
        public string AuthenticationToken { get; }
        
        public string ShopUsername { get; }

        public const string USER_UNAUTHORISED_ERROR = "You are not authorized to edit this business's connections.";

        public ConnectionDto(ErrorNotification errorNotes) : base(errorNotes)
        {
        }

        public ConnectionDto(string platformName, string authenticationToken, string shopUsername) {
            PlatformName = platformName;
            AuthenticationToken = authenticationToken;
            ShopUsername = shopUsername;
        }
    }
}