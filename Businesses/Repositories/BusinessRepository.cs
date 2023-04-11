using Amazon.DynamoDBv2.DataModel;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Exceptions;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Users.Models;

namespace instock_server_application.Businesses.Repositories; 

public class BusinessRepository : IBusinessRepository {

    private readonly IDynamoDBContext _context;

    public BusinessRepository(IDynamoDBContext context) {
        _context = context;
    }
    public async Task<BusinessDto?> GetBusiness(ValidateBusinessIdDto validateBusinessIdDto) {
        BusinessModel existingBusiness = await _context.LoadAsync<BusinessModel>(validateBusinessIdDto.BusinessId);

        if (existingBusiness == null) {
            return null;
        }
        
        BusinessDto businessDto = new BusinessDto(
            businessId: existingBusiness.BusinessId,
            businessName: existingBusiness.BusinessName,
            businessDescription: existingBusiness.BusinessDescription,
            businessOwnerId: existingBusiness.OwnerId,
            imageUrl: existingBusiness.ImageUrl
        );
        
        return businessDto;
    }

    public async Task<BusinessDto> SaveNewBusiness(StoreBusinessDto businessToSave) {
        
        // Checking the User Id is valid
        if (string.IsNullOrEmpty(businessToSave.UserId)) {
            throw new NullReferenceException("The User ID cannot be null or empty.");
        } 
        
        // Checking the Business Name is valid
        if (string.IsNullOrEmpty(businessToSave.BusinessName)) {
            throw new NullReferenceException("The Business Name cannot be null or empty.");
        }
        
        // Check if the user already has a business, throwing exception if so as we will update an existing business instead of create it
        // TODO Think this will be better to check the businesses not users by using a composite key when we database refactor
        User existingUser = await _context.LoadAsync<User>(businessToSave.UserId);
        if (!string.IsNullOrEmpty(existingUser.BusinessId)) {
            throw new UserAlreadyOwnsBusinessException();
        }
        
        // Save the new business
        BusinessModel businessModel = new BusinessModel(
            Guid.NewGuid(), 
            businessToSave.BusinessName, 
            businessToSave.UserId, 
            businessToSave.BusinessDescription,
            businessToSave.ImageUrl
        );
        await _context.SaveAsync(businessModel);
        
        // Update the user table to include new business
        existingUser.BusinessId = businessModel.BusinessId.ToString();
        await _context.SaveAsync(existingUser);

        BusinessDto createdBusiness = new BusinessDto(
            businessModel.BusinessId.ToString(), 
            businessModel.BusinessName, 
            businessModel.OwnerId.ToString(),
            businessModel.BusinessDescription,
            businessModel.ImageUrl
        );
        
        return createdBusiness;
    }
    
    public async Task<bool> DoesUserOwnABusiness(Guid userId) {
        
        // Checking the User Id is valid
        if (string.IsNullOrEmpty(userId.ToString())) {
            throw new NullReferenceException("The User ID cannot be null or empty.");
        }

        User existingUser = await _context.LoadAsync<User>(userId.ToString());

        return !string.IsNullOrEmpty(existingUser.BusinessId);
    }
    
    public async Task<StoreConnectionDto> SaveNewConnection(StoreConnectionDto storeConnectionDto) {
        // Validating details
        if (string.IsNullOrEmpty(storeConnectionDto.BusinessId)) {
            throw new NullReferenceException("The connection business ID cannot be null.");
        }
        
        
        Console.WriteLine("Phase 3");
        Console.WriteLine(storeConnectionDto.BusinessId);
        Console.WriteLine("Test");
        // Getting the existing connections

        
        
        ConnectionModel connection = new ConnectionModel(
        storeConnectionDto.BusinessId
            ); 
        
        Console.WriteLine("Phase 3.2");

        ConnectionModel existingConnections =
        await _context.LoadAsync<ConnectionModel>(connection.BusinessId);
        
        Console.WriteLine(existingConnections.ToString());
        
        Console.WriteLine("Phase 3.5");

        
        // Adding to the existing connections
        existingConnections.AddConnectionDetails("Test Shop", "Beep Boop Bap");

        Console.WriteLine("Phase 4");

        
        // Saving all of the updates
        await _context.SaveAsync(existingConnections);

        
        Console.WriteLine("Phase 5");
        
        // Return existing connections as store dto
        StoreConnectionDto allConnections = existingConnections.ToStoreConnectionDto();
        

        // Returning the new object that was saved
        return allConnections;
    }
    
    public async Task<StoreConnectionDto> GetConnections(string businessId)
    {
        // Validating details
        if (string.IsNullOrEmpty(businessId)) 
        {
            throw new NullReferenceException("The business ID cannot be null.");
        }
    
        // Getting the existing connections
        ConnectionModel connection = new ConnectionModel(businessId);
        ConnectionModel existingConnections = await _context.LoadAsync<ConnectionModel>(connection.BusinessId);
        StoreConnectionDto allConnections = existingConnections.ToStoreConnectionDto();
        
        // Returning the DTOs
        return allConnections;
    }
}