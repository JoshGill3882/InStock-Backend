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

    public async Task<bool> CreateBusiness(StoreBusinessDto businessToSave) {
        
        // Checking the User Id is valid
        if (string.IsNullOrEmpty(businessToSave.UserId)) {
            return false; // Invalid userId
        }
        
        // Check if the user already has a business
        // TODO We need to streamline our database process, get local versions working
        //User currentDbUser = await _context.LoadAsync<User>(userId);
        return true;
    }
    
}