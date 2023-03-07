using System.Security.Authentication;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Shared.Exceptions;
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
            throw new UserIdNullOrEmptyException();
        }
        
        // Check if the user already has a business
        // TODO Think this will be better to check the businesses not users by using a composite key when we database refactor
        User existingUser = await _context.LoadAsync<User>(businessToSave.UserId);
        
        // TODO Think the use of Business Exceptions here are creating a cross cutting concern and should be revisited and reworked
        if (!string.IsNullOrEmpty(existingUser.BusinessId)) {
            throw new UserAlreadyOwnsBusinessException();
        }
        
        // Save the new business
        BusinessModel businessModel = new BusinessModel(Guid.NewGuid(), businessToSave.BusinessName, businessToSave.UserId);
        await _context.SaveAsync(businessModel);
        
        // Update the user table to include new business
        existingUser.BusinessId = businessModel.BusinessId.ToString();
        await _context.SaveAsync(existingUser);

        BusinessDto createdBusiness = new BusinessDto(
            businessModel.BusinessId.ToString(), 
            businessModel.BusinessName, 
            businessModel.OwnerId.ToString());
        
        return createdBusiness;
    }
}