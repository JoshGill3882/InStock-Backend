using instock_server_application.Shared.Dto;
using System;

namespace instock_server_application.Businesses.Dtos
{
    public class ConnectionDto : DataTransferObjectSuperType
    {
        public string ShopName { get; }
        public string AuthenticationToken { get; }

        public const string USER_UNAUTHORIZED_ERROR = "You are not authorized to edit this business's connections.";

        public ConnectionDto(ErrorNotification errorNotes) : base(errorNotes)
        {
        }

        public ConnectionDto(string shopName, string authenticationToken)
        {
            ShopName = shopName;
            AuthenticationToken = authenticationToken;
        }
    }
}