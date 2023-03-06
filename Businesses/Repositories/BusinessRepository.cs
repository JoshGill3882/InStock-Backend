using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Users.Models;

namespace instock_server_application.Businesses.Repositories; 

public class BusinessRepository : IBusinessRepository {

    private readonly IDynamoDBContext _context;

    public BusinessRepository(IDynamoDBContext context) {
        _context = context;
    }

    public async Task<bool> SaveNewBusiness(StoreBusinessDto businessToSave) {
        
        // Checking the User Id is valid
        if (string.IsNullOrEmpty(businessToSave.UserId)) {
            return false; // Invalid userId
        }
        
        // Check if the user already has a business
        // TODO Think this will be better to check the businesses not users by using a composite key when we database refactor
        User existingUser = await _context.LoadAsync<User>(businessToSave.UserId);
        if (!string.IsNullOrEmpty(existingUser.BusinessId)) {
            return false;
        }
        
        // Save the new business
        BusinessModel businessModel = new BusinessModel(Guid.NewGuid(), businessToSave.BusinessName, businessToSave.UserId);
        await _context.SaveAsync(businessModel);
        
        // Update the user table to include new business
        existingUser.BusinessId = businessModel.BusinessId.ToString();
        await _context.SaveAsync(existingUser);
        
        return true;
    }
    
}