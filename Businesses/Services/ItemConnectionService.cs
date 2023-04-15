using instock_server_application.Businesses.Dtos;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class ItemConnectionService {
    private readonly ConnectionsService _connections;

    public ItemConnectionService(ConnectionsService connections) {
        _connections = connections;
    }

    public async Task ConnectItem(UserAuthorisationDto userAuthorisationDto, ItemConnectionRequestDto itemConnectionRequestDto) {
        
        // Validate the user can edit the business
        if (UserAuthorisationService.UserCanEditBusiness(userAuthorisationDto, itemConnectionRequestDto.BusinessId)) {
            ErrorNotification errorNotes = new ErrorNotification();
            errorNotes.AddError(UserAuthorisationDto.USER_UNAUTHORISED_ERROR);
            // TODO Return the error
        }
        
        // TODO Validates business is connected to this shop
        

        // TODO Validates Business contains Item SKU

        // TODO Validated Business Item doesn't already contain this connection 


        // TODO Validates Shop Connection contains Item SKU


        // TODO Stores Item's new connected item
        // TODO returns new connection
    }
    
}