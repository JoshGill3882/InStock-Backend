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

    public async Task<BusinessDto> SaveNewBusiness(StoreBusinessDto businessToSave) {
        
        // Checking the User Id is valid
        if (string.IsNullOrEmpty(businessToSave.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        } 
        
        // Checking the Business Name is valid
        if (string.IsNullOrEmpty(businessToSave.BusinessName)) {
            throw new NullReferenceException("The BusinessName cannot be null or empty.");
        }
        
        // Check if the user already has a business, throwing exception if so as we will update an existing business instead of create it
        // TODO Think this will be better to check the businesses not users by using a composite key when we database refactor
        User existingUser = await _context.LoadAsync<User>(businessToSave.UserId);
        if (!string.IsNullOrEmpty(existingUser.BusinessId)) {
            throw new UserAlreadyOwnsBusinessException();
        }
        
        // Save the new business
        BusinessModel businessModel = new BusinessModel(
            Guid.NewGuid(), businessToSave.BusinessName, businessToSave.UserId, businessToSave.BusinessDescription);
        await _context.SaveAsync(businessModel);
        
        // Update the user table to include new business
        existingUser.BusinessId = businessModel.BusinessId.ToString();
        await _context.SaveAsync(existingUser);

        BusinessDto createdBusiness = new BusinessDto(
            businessModel.BusinessId.ToString(), 
            businessModel.BusinessName, 
            businessModel.OwnerId.ToString(),
            businessModel.BusinessDescription);
        
        return createdBusiness;
    }
}