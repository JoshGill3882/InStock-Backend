using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos
{
    public class CreateConnectionRequestDto : DataTransferObjectSuperType
    {
        public string UserId { get; }
        public string UserBusinessId { get; }

        public string BusinessId { get; }
        public string PlatformName { get; }
        public string AuthenticationToken { get; }
        
        public string ShopUsername { get; }

        public CreateConnectionRequestDto(string userId, string userBusinessId, string businessId, string platformName, string authenticationToken, string shopUsername) {
            UserId = userId;
            UserBusinessId = userBusinessId;
            BusinessId = businessId;
            PlatformName = platformName;
            AuthenticationToken = authenticationToken;
            ShopUsername = shopUsername;
        }

        public CreateConnectionRequestDto(ErrorNotification errorNotes) : base(errorNotes) {
        }
        
        public GetConnectionsRequestDto ToGetConnectionsRequest()
        {
            return new GetConnectionsRequestDto(
                userId: UserId,
                userBusinessId: UserBusinessId,
                businessId: BusinessId
            );
        }
    }
}