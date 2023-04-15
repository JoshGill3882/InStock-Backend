using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class ItemConnectionService : IItemConnectionService {
    private readonly IConnectionsService _connections;

    public ItemConnectionService(IConnectionsService connections) {
        _connections = connections;
    }

    public async Task ConnectItem(UserAuthorisationDto userAuthorisationDto, ItemConnectionRequestDto itemConnectionRequestDto) {
        ErrorNotification errorNotes = new ErrorNotification();

        // Validate the user can edit the business
        if (!UserAuthorisationService.UserCanEditBusiness(userAuthorisationDto, itemConnectionRequestDto.BusinessId)) {
            errorNotes.AddError(UserAuthorisationDto.USER_UNAUTHORISED_ERROR);
            // TODO Return the error
            return;
        }
        
        // TODO Validates business is connected to this shop
        if (!await _connections.ValidateBusinessConnectedToPlatform(userAuthorisationDto, itemConnectionRequestDto.BusinessId,
                itemConnectionRequestDto.PlatformName)) {
            errorNotes.AddError("You are not connected to this platform.");
            // TODO Return the error
            return;
        }

        // TODO Validates Business contains Item SKU

        // TODO Validated Business Item doesn't already contain this connection 


        // TODO Validates Shop Connection contains Item SKU


        // TODO Stores Item's new connected item
        // TODO returns new connection
    }
    
}