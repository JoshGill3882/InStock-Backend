using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Exceptions;
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
        
        // Check if the user already has an item of the same name, throwing exception if so
        // Item existingItem = await _context.LoadAsync<Item>(itemToSaveDto.Name);
        // if (!string.IsNullOrEmpty(existingItem.BusinessId)) {
        //     throw new UserAlreadyOwnsBusinessException();
        // }
    
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
    
    public async Task<bool> IsNameInUse(CreateItemRequestDto createItemRequestDto)
    {
        var duplicateName = false;
        var request = new ScanRequest
        {
            TableName = "Items",
            ExpressionAttributeValues = new Dictionary<string,AttributeValue> {
                {":Id", new AttributeValue(createItemRequestDto.BusinessId)},
                {":name", new AttributeValue(createItemRequestDto.Name)}
            },
            // "Name" is protected in DynamoDB so Expression Attribute Name is required
            ExpressionAttributeNames = new Dictionary<string,string> {
                {"#n", "Name"},
            },

            FilterExpression = "BusinessId = :Id and #n = :name",
        };
        
        var response = await _client.ScanAsync(request);
        if (response.Items.Count > 0)
        {
            duplicateName = true;
        }
        return duplicateName;
    }

}