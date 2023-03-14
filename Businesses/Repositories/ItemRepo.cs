using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Exceptions;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Users.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Rewrite;

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
            itemToSaveDto.SKU, itemToSaveDto.BusinessId, itemToSaveDto.Category, itemToSaveDto.Name, Int32.Parse(itemToSaveDto.Stock));
        await _context.SaveAsync(itemModel);

        ItemDto createdItemDto = new ItemDto(
            itemModel.SKU, 
            itemModel.BusinessId, 
            itemModel.Category,
            itemModel.Name,
            itemModel.Stock.ToString());
        
        return createdItemDto;
    }
    
    public async Task<bool> IsNameInUse(string businessId, string itemName)
    {
        var duplicateName = false;
        var request = new ScanRequest
        {
            TableName = "Items",
            ExpressionAttributeValues = new Dictionary<string,AttributeValue> {
                {":Id", new AttributeValue(businessId)},
                {":name", new AttributeValue(itemName)}
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

    public async Task<ItemDto> GetItemWithUpdate(string itemId, string businessId, JsonPatchDocument patchDocument) {
        Item existingItem = await _context.LoadAsync<Item>(itemId, businessId);

        patchDocument.ApplyTo(existingItem);
        
        ItemDto createdItemDto = new ItemDto(
            existingItem.SKU, 
            existingItem.BusinessId, 
            existingItem.Category,
            existingItem.Name,
            existingItem.Stock.ToString());

        return createdItemDto;
    }

    public async Task<ItemDto> SaveExistingItem(StoreItemDto itemToSaveDto) {
                
        // Checking the Item SKU is valid
        if (string.IsNullOrEmpty(itemToSaveDto.SKU)) {
            throw new NullReferenceException("The Item SKU cannot be null or empty.");
        }
        
        // Checking the Item Name is valid
        if (string.IsNullOrEmpty(itemToSaveDto.Name)) {
            throw new NullReferenceException("The Item Name cannot be null or empty.");
        }
        
        // Check if the item already exists so we know we are updating one
        Item existingItem = await _context.LoadAsync<Item>(itemToSaveDto.SKU);

        if (existingItem == null) {
            throw new NullReferenceException("The Item you are trying to update does not exist");
        }
        
        // Save the new updated item
        Item itemModel = new Item(
            itemToSaveDto.SKU, itemToSaveDto.BusinessId, itemToSaveDto.Category, itemToSaveDto.Name, Int32.Parse(itemToSaveDto.Stock));
        await _context.SaveAsync(itemModel);
        
        // Return the updated Items details
        ItemDto updatedItemDto = new ItemDto(
            itemModel.SKU, 
            itemModel.BusinessId, 
            itemModel.Category,
            itemModel.Name,
            itemModel.Stock.ToString());
        
        return updatedItemDto;
    }
}