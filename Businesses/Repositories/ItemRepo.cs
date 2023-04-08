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
            TableName = Item.TableName,
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
            itemToSaveDto.SKU, 
            itemToSaveDto.BusinessId, 
            itemToSaveDto.Category,
            itemToSaveDto.Name,
            itemToSaveDto.Stock,
            itemToSaveDto.ImageUrl
        );
        await _context.SaveAsync(itemModel);

        ItemDto createdItemDto = new ItemDto(
            itemModel.SKU, 
            itemModel.BusinessId, 
            itemModel.Category,
            itemModel.Name,
            itemModel.GetStock(),
            itemModel.ImageUrl
        );
        
        return createdItemDto;
    }
    
    public async Task<bool> IsNameInUse(CreateItemRequestDto createItemRequestDto)
    {
        var duplicateName = false;
        var request = new ScanRequest
        {
            TableName = Item.TableName,
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
            TableName = Item.TableName,
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
        Item item = new Item(deleteItemDto.ItemId, deleteItemDto.BusinessId);
        _context.DeleteAsync(item);
    }
    
    public async Task<List<Dictionary<string, AttributeValue>>> GetAllCategories(ValidateBusinessIdDto validateBusinessIdDto) {
        var request = new ScanRequest
        {
            TableName = Item.TableName,
            ProjectionExpression = "Category",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":Id", new AttributeValue(validateBusinessIdDto.BusinessId)}
            },
            FilterExpression = "BusinessId = :Id",
        };
    
        var response = await _client.ScanAsync(request);
    
        var categories = new HashSet<string>();
        var categoryList = new List<Dictionary<string, AttributeValue>>();
        foreach (var item in response.Items) {
            var categoryValue = item["Category"];
            var category = categoryValue.S;
            if (categories.Add(category)) {
                categoryList.Add(new Dictionary<string, AttributeValue> {
                    {"Category", categoryValue}
                });
            }
        }
    
        return categoryList;
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
        Item existingItem = await _context.LoadAsync<Item>(itemToSaveDto.SKU, itemToSaveDto.BusinessId);

        if (existingItem == null) {
            throw new NullReferenceException("The Item you are trying to update does not exist");
        }
        
        // Save the new updated item
        Item itemModel = new Item(
            itemToSaveDto.SKU, 
            itemToSaveDto.BusinessId,
            itemToSaveDto.Category,
            itemToSaveDto.Name,
            itemToSaveDto.Stock,
            itemToSaveDto.ImageUrl
        );
        await _context.SaveAsync(itemModel);
        
        // Return the updated Items details
        ItemDto updatedItemDto = new ItemDto(
            itemModel.SKU, 
            itemModel.BusinessId, 
            itemModel.Category,
            itemModel.Name,
            itemModel.GetStock(),
            itemModel.ImageUrl
        );
        
        return updatedItemDto;
    }

    public async Task<StockUpdateDto> SaveStockUpdate(StoreStockUpdateDto storeStockUpdateDto) {
        // Validating details
        if (string.IsNullOrEmpty(storeStockUpdateDto.BusinessId)) {
            throw new NullReferenceException("The stock update business ID cannot be null.");
        }
        if (string.IsNullOrEmpty(storeStockUpdateDto.ItemSku)) {
            throw new NullReferenceException("The stock update item ID cannot be null.");
        }

        // Getting the existing items stock updates
        ItemStockUpdateModel existingStockUpdates =
            await _context.LoadAsync<ItemStockUpdateModel>(storeStockUpdateDto.ItemSku, storeStockUpdateDto.BusinessId);

        // Adding to the existing stock updates
        existingStockUpdates.AddStockUpdateDetails(storeStockUpdateDto.ChangeStockAmountBy, storeStockUpdateDto.ReasonForChange, storeStockUpdateDto.DateTimeAdded);

        // Saving all of the updates
        await _context.SaveAsync(existingStockUpdates);

        // Returning the new object that was saved
        StockUpdateDto stockUpdateDto =
            new StockUpdateDto(storeStockUpdateDto.ChangeStockAmountBy, storeStockUpdateDto.ReasonForChange, storeStockUpdateDto.DateTimeAdded);
        
        return stockUpdateDto;
    }

    public async Task<ItemDto?> GetItem(string businessId, string itemSku) {
        // Validating details
        if (string.IsNullOrEmpty(businessId)) {
            throw new NullReferenceException("The stock update business ID cannot be null.");
        }
        if (string.IsNullOrEmpty(itemSku)) {
            throw new NullReferenceException("The stock update item ID cannot be null.");
        }

        // Getting the existing item
        Item item = await _context.LoadAsync<Item>(itemSku, businessId);

        // If the item wasn't in the database then return null
        if (item == null) {
            return null;
        }
        
        // Returning the item details from the database
        ItemDto itemDto = new ItemDto(item.SKU, item.BusinessId, item.Category, item.Name, item.GetStock(), item.ImageUrl);
        return itemDto;
    }
}