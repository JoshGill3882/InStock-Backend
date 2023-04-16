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

    public async Task<ItemConnectionsDto> ConnectItem(UserAuthorisationDto userAuthorisationDto, ItemConnectionRequestDto itemConnectionRequestDto) {
        ErrorNotification errorNotes = new ErrorNotification();

        // Validate the user can edit the business
        if (!UserAuthorisationService.UserCanEditBusiness(userAuthorisationDto, itemConnectionRequestDto.BusinessId)) {
            errorNotes.AddError(UserAuthorisationDto.USER_UNAUTHORISED_ERROR);
            return new ItemConnectionsDto(errorNotes);
        }
        
        StoreConnectionDto currentBusinessConnections = await _connections.GetConnections(new GetConnectionsRequestDto(
            userAuthorisationDto.UserId, userAuthorisationDto.UserBusinessId, itemConnectionRequestDto.BusinessId));

        // Validate that we support the connection
        ConnectionDto? connectedPlatform = null;
        foreach (ConnectionDto connection in currentBusinessConnections.Connections) {
            if (connection.PlatformName.ToLower().Equals(itemConnectionRequestDto.PlatformName.ToLower())) {
                connectedPlatform = connection;
            }
        }
        if (connectedPlatform == null) {
            errorNotes.AddError("We do not support integration with this platform.");
            return new ItemConnectionsDto(errorNotes);
        }
        
        // Validates business is connected to this shop
        if (!await _connections.ValidateBusinessConnectedToPlatform(userAuthorisationDto, itemConnectionRequestDto.BusinessId,
                itemConnectionRequestDto.PlatformName)) {
            errorNotes.AddError("You are not connected to this platform.");
            return new ItemConnectionsDto(errorNotes);
        }

        ItemConnectionsDto? existingItemConnections =
            await _itemRepo.GetItemConnections(itemConnectionRequestDto.BusinessId, itemConnectionRequestDto.ItemSku)!;
        
        // Validates Business contains Item SKU
        if (existingItemConnections == null!) {
            errorNotes.AddError($"The item {itemConnectionRequestDto.ItemSku} does not exist.");
            return new ItemConnectionsDto(errorNotes);
        }
        
        // Validated Business Item doesn't already contain this connection 
        if (existingItemConnections.Connections.ContainsKey(itemConnectionRequestDto.PlatformName)) {
            errorNotes.AddError($"The item {itemConnectionRequestDto.ItemSku} is already connected to {itemConnectionRequestDto.PlatformName}.");
        }

        // Validates Shop Connection contains Item SKU
        ExternalShopConnectorService shopConnection = ExternalServiceConnectorFactory.CreateConnector(itemConnectionRequestDto.PlatformName);
        if (!await shopConnection.HasItemSku(connectedPlatform.ShopUsername, itemConnectionRequestDto.PlatformItemSku)) {
            errorNotes.AddError($"The item {itemConnectionRequestDto.PlatformItemSku} does not exist on {itemConnectionRequestDto.PlatformName}.");
        }

        if (errorNotes.HasErrors) {
            return new ItemConnectionsDto(errorNotes);
        }
        
        // Stores Item's new connected item
        ItemConnectionsDto itemConnectionsDtoToStore = existingItemConnections;
        itemConnectionsDtoToStore.Connections[itemConnectionRequestDto.PlatformName] =
            itemConnectionRequestDto.PlatformItemSku;

        ItemConnectionsDto storedConnections = await _itemRepo.SaveItemConnections(itemConnectionsDtoToStore);

        return storedConnections;
    }

    public async Task<ListOfConnectedItemDetailsDto> GetItemConnectionsDetails(UserAuthorisationDto userAuthorisationDto, ItemRequestDto itemRequestDto) {
        ErrorNotification errorNotes = new ErrorNotification();
        
        // Validate the user can edit the business
        if (!UserAuthorisationService.UserCanEditBusiness(userAuthorisationDto, itemRequestDto.BusinessId)) {
            errorNotes.AddError(UserAuthorisationDto.USER_UNAUTHORISED_ERROR);
            return new ListOfConnectedItemDetailsDto(errorNotes);
        }
        
        ItemConnectionsDto? existingItemConnections =
            await _itemRepo.GetItemConnections(itemRequestDto.BusinessId, itemRequestDto.Sku)!;

        if (existingItemConnections.Connections.Count == 0) {
            return new ListOfConnectedItemDetailsDto(new List<ConnectedItemDetailsDto>());
        }

        // Getting all the business connections for their usernames
        StoreConnectionDto businessConnections = await _connections.GetConnections(
            new GetConnectionsRequestDto(userAuthorisationDto.UserId, userAuthorisationDto.UserBusinessId,
                itemRequestDto.BusinessId));
        
        // Adding all the connections to a lookup table so that we can grab them when looping the item's connections
        Dictionary<string, string> platformUsernameLookup = new Dictionary<string, string>();
        foreach (ConnectionDto connection in businessConnections.Connections) {
            platformUsernameLookup.Add(connection.PlatformName, connection.ShopUsername);
        }

        List<ConnectedItemDetailsDto> connectedItemDetailsDtos = new List<ConnectedItemDetailsDto>();

        foreach (string connection in existingItemConnections.Connections.Keys) {
            ExternalShopConnectorService externalConnection = ExternalServiceConnectorFactory.CreateConnector(connection);
            connectedItemDetailsDtos.Add(await externalConnection.GetConnectedItemDetails(platformUsernameLookup[connection],
                existingItemConnections.Connections[connection]));
        }

        return new ListOfConnectedItemDetailsDto(connectedItemDetailsDtos);
    }
    
}