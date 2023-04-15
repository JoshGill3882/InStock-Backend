using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos
{
    public class UserAuthorisationDto : DataTransferObjectSuperType
    {
        public string UserId { get; }
        public string UserBusinessId { get; }
        
        public const string USER_UNAUTHORISED_ERROR = "You are not authorized to edit this business's connections.";

        public UserAuthorisationDto(string userId, string userBusinessId) {
            UserId = userId;
            UserBusinessId = userBusinessId;
        }
    }
}