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
        
        // Checking the Business Name is valid
        if (string.IsNullOrEmpty(itemToSaveDto.Name)) {
            throw new NullReferenceException("The Business Name cannot be null or empty.");
        }
        
        // Save the new item
        Item itemModel = new Item(
            itemToSaveDto.SKU, itemToSaveDto.BusinessId, itemToSaveDto.Category, itemToSaveDto.Name, itemToSaveDto.Stock);
        await _context.SaveAsync(itemModel);

        ItemDto createdItemDto = new ItemDto(
            itemModel.SKU, 
            itemModel.BusinessId, 
            itemModel.Category,
            itemModel.Name,
            itemModel.Stock);
        
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
    
    public async Task<bool> IsSKUInUse(string SKU, string businessId)
    {

        var duplicateSKU = false;
        
        var request = new ScanRequest
        {
            TableName = "Items",
            ExpressionAttributeValues = new Dictionary<string,AttributeValue> {
                {":Id", new AttributeValue(businessId)},
                {":SKU", new AttributeValue(SKU)}
            },

            FilterExpression = "BusinessId = :Id and SKU = :SKU",
        };
        
        var response = await _client.ScanAsync(request);
        if (response.Items.Count > 0)
        {
            duplicateSKU = true;
        }

        return duplicateSKU;

    }

    public void Delete(DeleteItemDto deleteItemDto) {
        _context.DeleteAsync(deleteItemDto.ItemId);
    }
}