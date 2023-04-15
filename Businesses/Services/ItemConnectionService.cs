using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Abstractions;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class ItemConnectionService : IItemConnectionService {
    private readonly IConnectionsService _connections;
    private readonly IItemRepo _itemRepo;

    public ItemConnectionService(IConnectionsService connections, IItemRepo itemRepo) {
        _connections = connections;
        _itemRepo = itemRepo;
    }

    public async Task ConnectItem(UserAuthorisationDto userAuthorisationDto, ItemConnectionRequestDto itemConnectionRequestDto) {
        ErrorNotification errorNotes = new ErrorNotification();

        // Validate the user can edit the business
        if (!UserAuthorisationService.UserCanEditBusiness(userAuthorisationDto, itemConnectionRequestDto.BusinessId)) {
            errorNotes.AddError(UserAuthorisationDto.USER_UNAUTHORISED_ERROR);
            // TODO Return the error
            return;
        }
        
        // Validate that we support the connection
        if (ExternalServiceConnectorFactory.ValidatePlatformName(itemConnectionRequestDto.PlatformName)) {
            errorNotes.AddError("We do not support integration with this platform.");
            // TODO Return the error
            return;
        }
        
        // Validates business is connected to this shop
        if (!await _connections.ValidateBusinessConnectedToPlatform(userAuthorisationDto, itemConnectionRequestDto.BusinessId,
                itemConnectionRequestDto.PlatformName)) {
            errorNotes.AddError("You are not connected to this platform.");
            // TODO Return the error
            return;
        }

        ItemConnectionsDto? existingItemConnections =
            await _itemRepo.GetItemConnections(itemConnectionRequestDto.BusinessId, itemConnectionRequestDto.ItemSku)!;
        
        // Validates Business contains Item SKU
        if (existingItemConnections == null!) {
            errorNotes.AddError($"The item {itemConnectionRequestDto.ItemSku} does not exist.");
            // TODO Return the error
            return;
        }
        
        // Validated Business Item doesn't already contain this connection 
        if (existingItemConnections.Connections.ContainsKey(itemConnectionRequestDto.PlatformName)) {
            errorNotes.AddError($"The item {itemConnectionRequestDto.ItemSku} is already connected to {itemConnectionRequestDto.PlatformName}.");
        }

        // Validates Shop Connection contains Item SKU
        

        // TODO Stores Item's new connected item
        // TODO returns new connection
    }
    
}