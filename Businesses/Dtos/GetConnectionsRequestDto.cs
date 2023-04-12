using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos
{
    public class GetConnectionsRequestDto : DataTransferObjectSuperType
    {
        public string UserId { get; }
        
        public string UserBusinessId { get; }

        public string BusinessId { get; }

        public GetConnectionsRequestDto(string userId, string userBusinessId, string businessId) {
            UserId = userId;
            UserBusinessId = userBusinessId;
            BusinessId = businessId;
        }
    }
}