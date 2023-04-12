using System.Text;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Abstractions;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace instock_server_application.Businesses.Services; 

public class ConnectionsService {
    private readonly IBusinessRepository _businessRepository;

    public ConnectionsService(IBusinessRepository businessRepository)
    {
        _businessRepository = businessRepository;
    }
    
    public async Task<StoreConnectionDto> CreateConnections(CreateConnectionRequestDto createConnectionRequestDto) {
        
        // Validation
        // Check if the user and business Ids are valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(createConnectionRequestDto.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(createConnectionRequestDto.UserBusinessId)) {
            throw new NullReferenceException("The BusinessId cannot be null or empty.");
        }
        
        ErrorNotification errorNotes = new ErrorNotification();
        
        // Check if the user is allowed to edit the business, return as no need to do anymore validation
        if (!createConnectionRequestDto.UserBusinessId.Equals(createConnectionRequestDto.BusinessId)) {
            errorNotes.AddError(StoreConnectionDto.USER_UNAUTHORISED_ERROR);
            return new StoreConnectionDto(errorNotes);
        }
        
        //
        // // If we have had any errors then return them
        // if (errorNotes.HasErrors) {
        //     return new StockUpdateDto(errorNotes);
        // }
        
        // Saving to database
        // Create new Stock Update record
        
        List<ConnectionDto> connectionDtos = new List<ConnectionDto>();

        ConnectionDto connectionDto = new ConnectionDto(
            shopName: createConnectionRequestDto.ShopName,
            authenticationToken: createConnectionRequestDto.AuthenticationToken
        );
        connectionDtos.Add(connectionDto);
        
        StoreConnectionDto storeConnectionDtoToSave = new StoreConnectionDto(
            businessId: createConnectionRequestDto.BusinessId,
            connections: connectionDtos
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
        
        ErrorNotification errorNotes = new ErrorNotification();
        
        // Check if the user is allowed to edit the business, return as no need to do anymore validation
        if (!getConnectionsRequestDto.UserBusinessId.Equals(getConnectionsRequestDto.BusinessId)) {
            errorNotes.AddError(StoreConnectionDto.USER_UNAUTHORISED_ERROR);
            return new StoreConnectionDto(errorNotes);
        }
        
        StoreConnectionDto storeConnectionDto = await _businessRepository.GetConnections(getConnectionsRequestDto.BusinessId);

        
        return storeConnectionDto;
    }

    public async Task<String> ConnectToExternalShop(CreateConnectionForm connectionRequestDetails) {

        //Create a class for calling with an interface
        Console.WriteLine("Connecting");
        ExternalServiceConnectorFactory externalServiceConnectorFactory = new ExternalServiceConnectorFactory();
        ExternalShopAuthenticator authenticator =
            externalServiceConnectorFactory.CreateAuthenticator(connectionRequestDetails);

        ExternalShopLoginDto loginDetails = new ExternalShopLoginDto(
            shopUsername: connectionRequestDetails.ShopUsername,
            shopUserPassword: connectionRequestDetails.ShopUserPassword
        );
        var res = await authenticator.LoginToShop(loginDetails);
        Console.WriteLine("Ree working");
        Console.WriteLine(res);
        return res;
    }
}
   
