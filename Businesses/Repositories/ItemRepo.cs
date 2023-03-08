using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;

namespace instock_server_application.Businesses.Repositories; 

public class ItemRepo : IItemRepo{
    private readonly IAmazonDynamoDB _client;
    private readonly IDynamoDBContext _context;

    public ItemRepo(IAmazonDynamoDB client, IDynamoDBContext context) {
        _client = client;
        _context = context;
    }
    public async Task<List<Dictionary<string, AttributeValue>>> GetAllItems(string businessId) {
        var request = new QueryRequest {
            TableName = "Items",
            IndexName = "BusinessId",
            KeyConditionExpression = "BusinessId = :Id",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":Id", new AttributeValue(businessId)}
            }
        };
        var response = await _client.QueryAsync(request);
        return response.Items;
    }
    
    public async Task<ItemDto> SaveNewItem(StoreItemDto itemToSaveDto) {
        
        // Checking the User Id is valid
        // if (string.IsNullOrEmpty(itemToSaveDto.UserId)) {
        //     throw new NullReferenceException("The User ID cannot be null or empty.");
        // } 
        
        // Checking the Business Name is valid
        if (string.IsNullOrEmpty(itemToSaveDto.Name)) {
            throw new NullReferenceException("The Business Name cannot be null or empty.");
        }
        
        // Check if the user already has a business, throwing exception if so as we will update an existing business instead of create it
        // TODO Think this will be better to check the businesses not users by using a composite key when we database refactor
        // User existingUser = await _context.LoadAsync<User>(businessToSave.UserId);
        // if (!string.IsNullOrEmpty(existingUser.BusinessId)) {
        //     throw new UserAlreadyOwnsBusinessException();
        // }
        //
        // Save the new business
        Item itemModel = new Item(
            itemToSaveDto.SKU, itemToSaveDto.BusinessId, itemToSaveDto.Category, itemToSaveDto.Name, Int32.Parse(itemToSaveDto.Stock));
        await _context.SaveAsync(itemModel);
        
        // Update the user table to include new business
        // existingUser.BusinessId = businessModel.BusinessId.ToString();
        // await _context.SaveAsync(existingUser);

        ItemDto createdItemDto = new ItemDto(
            itemModel.SKU.ToString(), 
            itemModel.BusinessId.ToString(), 
            itemModel.Category.ToString(),
            itemModel.Name.ToString(),
            itemModel.Stock.ToString());
        
        return createdItemDto;
    }
}