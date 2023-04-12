using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos
{
    public class CreateConnectionRequestDto : DataTransferObjectSuperType
    {
        public string UserId { get; }
        public string UserBusinessId { get; }

        public string BusinessId { get; }
        public string ShopName { get; }
        public string AuthenticationToken { get; }

        public CreateConnectionRequestDto(string userId, string userBusinessId, string businessId, string shopName, string authenticationToken)
        {
            UserId = userId;
            UserBusinessId = userBusinessId;
            BusinessId = businessId;
            ShopName = shopName;
            AuthenticationToken = authenticationToken;
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