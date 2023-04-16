using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ListOfConnectedItemDetailsDto : DataTransferObjectSuperType {

    public List<ConnectedItemDetailsDto> ListOfConnectedItemDetails;
    
    public ListOfConnectedItemDetailsDto(ErrorNotification errorNotes) : base(errorNotes) {}

    public ListOfConnectedItemDetailsDto(List<ConnectedItemDetailsDto> listOfConnectedItemDetails) {
        ListOfConnectedItemDetails = listOfConnectedItemDetails;
    }
}