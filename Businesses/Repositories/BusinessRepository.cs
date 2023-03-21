using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Exceptions;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Users.Models;

namespace instock_server_application.Businesses.Repositories; 

public class BusinessRepository : IBusinessRepository {

    private readonly IAmazonDynamoDB _client;
    private readonly IDynamoDBContext _context;

    public BusinessRepository(IAmazonDynamoDB client, IDynamoDBContext context) {
        _client = client;
        _context = context;
    }
    
    public async Task<Dictionary<string, AttributeValue>> GetBusiness(ValidateBusinessIdDto validateBusinessIdDto) {
        var request = new GetItemRequest() {
            TableName = BusinessModel.TableName,
            Key = new Dictionary<string,AttributeValue>() {
                {
                    "BusinessId", new AttributeValue { S = validateBusinessIdDto.BusinessId }
                }
            },
        };
        var response = await _client.GetItemAsync(request);
        return response.Item;
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
    
    public async Task<bool> DoesUserOwnABusiness(Guid userId) {
        
        // Checking the User Id is valid
        if (string.IsNullOrEmpty(userId.ToString())) {
            throw new NullReferenceException("The User ID cannot be null or empty.");
        }

        User existingUser = await _context.LoadAsync<User>(userId.ToString());

        return !string.IsNullOrEmpty(existingUser.BusinessId);
    }
}