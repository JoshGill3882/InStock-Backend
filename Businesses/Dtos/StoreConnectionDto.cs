
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos
{
    public class StoreConnectionDto : DataTransferObjectSuperType
    {
        public string BusinessId { get; }
        public List<ConnectionDto> Connections { get; }
        
        public const string USER_UNAUTHORISED_ERROR = "You are not authorized to edit this business's connections.";

        public StoreConnectionDto(ErrorNotification errorNotes) : base(errorNotes)
        {
        }

        public StoreConnectionDto(string businessId, List<ConnectionDto> connections) {
            BusinessId = businessId;
            Connections = connections;
        }
    }
}