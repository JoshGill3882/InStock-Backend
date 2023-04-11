using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class ConnectionsService {
    private readonly IBusinessRepository _businessRepository;

    public ConnectionsService(IBusinessRepository businessRepository) {
        _businessRepository = businessRepository;
    }
    
    public async Task<StoreConnectionDto> CreateConnection(CreateConnectionRequestDto createConnectionRequestDto) {
        
        Console.WriteLine("Phase 0.5");
        Console.WriteLine(createConnectionRequestDto.BusinessId);
        
        
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
        
        Console.WriteLine("Phase 1");

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

        Console.WriteLine(storeConnectionDtoToSave.BusinessId);
        
        Console.WriteLine("Phase 2");

        StoreConnectionDto storeConnectionDto = await _businessRepository.SaveNewConnection(storeConnectionDtoToSave);
        
        // Returning results
        // Return newly created stock update
        return storeConnectionDto;
    }
    
    
    //get all connections 
    
    //save new connection
    // Method to get all connections 
    
   // Method to add connection to users account 
   
   // Method to attempt sign in to mock shop
   // List of connection objects 
   // connection name (shop)
   // connection token
   
}