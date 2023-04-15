using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Abstractions;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class ConnectionsService : IConnectionsService {
    private readonly IBusinessRepository _businessRepository;

    public ConnectionsService(IBusinessRepository businessRepository)
    {
        _businessRepository = businessRepository;
    }
    
    public async Task<StoreConnectionDto> CreateConnections(CreateConnectionRequestDto createConnectionRequestDto) {
        
        // Validation
        ValidateConnectionRequest(createConnectionRequestDto);
        
        ErrorNotification errorNotes = new ErrorNotification();
        
        // Check if the user is allowed to edit the business, return as no need to do anymore validation
        if (!createConnectionRequestDto.UserBusinessId.Equals(createConnectionRequestDto.BusinessId)) {
            errorNotes.AddError(StoreConnectionDto.USER_UNAUTHORISED_ERROR);
            return new StoreConnectionDto(errorNotes);
        }

        GetConnectionsRequestDto getConnectionsRequestDto = createConnectionRequestDto.ToGetConnectionsRequest();
        StoreConnectionDto connections = await GetConnections(getConnectionsRequestDto);
        // Get connections
        List<ConnectionDto> connectionsList = connections.Connections;
        
        ConnectionDto connectionDto = new ConnectionDto(
            platformName: createConnectionRequestDto.PlatformName,
            authenticationToken: createConnectionRequestDto.AuthenticationToken,
            shopUsername: createConnectionRequestDto.ShopUsername
        );
        
        //Check for duplicates
        foreach (ConnectionDto existingConnection in connectionsList)
        {
            if (existingConnection.PlatformName == connectionDto.PlatformName)
            {
                errorNotes.AddError("You are already connected to this shop.");
                return new StoreConnectionDto(errorNotes);
            }
        }
        
        connectionsList.Add(connectionDto);
        
        StoreConnectionDto storeConnectionDtoToSave = new StoreConnectionDto(
            businessId: createConnectionRequestDto.BusinessId,
            connections: connectionsList
        );
        
        StoreConnectionDto storeConnectionDto = await _businessRepository.SaveNewConnection(storeConnectionDtoToSave);
        
        // Returning results
        // Return newly created stock update
        return storeConnectionDto;
    }
    
    
    public async Task<StoreConnectionDto> GetConnections(GetConnectionsRequestDto getConnectionsRequestDto) {
        // Validation
        // Check if the user and business Ids are valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(getConnectionsRequestDto.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(getConnectionsRequestDto.UserBusinessId)) {
            throw new NullReferenceException("The BusinessId cannot be null or empty.");
        }
        
        
        // Check if the user is allowed to edit the business, return as no need to do anymore validation
        if (!getConnectionsRequestDto.UserBusinessId.Equals(getConnectionsRequestDto.BusinessId)) {
            ErrorNotification errorNotes = new ErrorNotification();
            errorNotes.AddError("You do not have permissions to interact with this businesses connections");
            return new StoreConnectionDto(errorNotes);
        }
        
        StoreConnectionDto storeConnectionDto = await _businessRepository.GetConnections(getConnectionsRequestDto.BusinessId);

        
        return storeConnectionDto;
    }

    public async Task<ExternalShopAuthenticationTokenDto> ConnectToExternalShop(CreateConnectionForm connectionRequestDetails) {

        //Create a class for calling with an interface
        try { 
            ExternalShopConnectorService connectorService =
            ExternalServiceConnectorFactory.CreateConnector(connectionRequestDetails);

            ExternalShopLoginDto loginDetails = new ExternalShopLoginDto(
                shopUsername: connectionRequestDetails.ShopUsername,
                shopUserPassword: connectionRequestDetails.ShopUserPassword
            );
            ExternalShopAuthenticationTokenDto authenticationToken = await connectorService.LoginToShop(loginDetails);
            return authenticationToken;
        }
        catch (Exception e) {
            ErrorNotification errorNote = new ErrorNotification();
            errorNote.AddError(e.Message);
            return new ExternalShopAuthenticationTokenDto(errorNote);
        }
    }

    public async Task<bool> ValidateBusinessConnectedToPlatform(UserAuthorisationDto userAuthorisationDto, string businessId, string platformName) {
        StoreConnectionDto currentConnections = await GetConnections(new GetConnectionsRequestDto(
            userAuthorisationDto.UserId, userAuthorisationDto.UserBusinessId, businessId));
        
        foreach (ConnectionDto connection in currentConnections.Connections) {
            if (connection.PlatformName.ToLower().Equals(platformName.ToLower())) {
                return true;
            }
        }

        return false;
    }
    
    private void ValidateConnectionRequest(CreateConnectionRequestDto createConnectionRequestDto)
    {
        if (string.IsNullOrEmpty(createConnectionRequestDto.UserId))
        {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(createConnectionRequestDto.UserBusinessId))
        {
            throw new NullReferenceException("The BusinessId cannot be null or empty.");
        }
    }
    
}
   
