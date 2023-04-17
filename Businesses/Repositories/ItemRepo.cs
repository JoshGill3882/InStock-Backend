using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
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
    
    public async Task<List<ItemDto>> GetAllItems(string businessId) {
        
        // Get List of items
        List<Item> listOfItemModel = await _context.ScanAsync<Item>(
            new [] {
                Item.ByBusinessId(businessId)
            }).GetRemainingAsync();

        // Convert list of items
        List<ItemDto> listOfItemDto = new List<ItemDto>();
        
        foreach (Item itemModel in listOfItemModel) {
            listOfItemDto.Add(
                new ItemDto(
                    sku: itemModel.SKU,
                    businessId: itemModel.BusinessId,
                    category: itemModel.Category,
                    name: itemModel.Name,
                    totalStock: itemModel.GetTotalStock(),
                    totalOrders: itemModel.GetTotalOrders(),
                    availableStock: itemModel.GetTotalStock() - itemModel.GetTotalOrders(),
                    imageFilename: itemModel.ImageFilename));
        }
        
        return listOfItemDto;
    }    
    
    public async Task<List<StatItemDto>> GetAllItemsStatsDetails(string businessId) {
        
        // Get List of items
        List<ItemStatsDetailsModel> listOfItemModel = await _context.ScanAsync<ItemStatsDetailsModel>(
            new [] {
                ItemStatsDetailsModel.ByBusinessId(businessId)
            }).GetRemainingAsync();

        // Convert list of items
        List<StatItemDto> listOfStatItemDto = new List<StatItemDto>();
        
        foreach (ItemStatsDetailsModel itemModel in listOfItemModel) {

            List<StatStockDto> listOfStatStockDtos = new List<StatStockDto>();
            
            if (itemModel.StockUpdates != null) {
                foreach (ItemStockUpdateModel.StockUpdateObject stockObject in itemModel.StockUpdates) {
                    StatStockDto statStockDto = new StatStockDto(stockObject.AmountChanged, stockObject.ReasonForChange,
                        stockObject.DateTimeAdded.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff"));
                    listOfStatStockDtos.Add(statStockDto);
                }
            }

            // Move this within the above if statement
            listOfStatItemDto.Add(
                new StatItemDto(
                    sku: itemModel.SKU,
                    businessId: itemModel.BusinessId,
                    category: itemModel.Category,
                    name: itemModel.Name,
                    stock: itemModel.TotalStock,
                    stockUpdates: listOfStatStockDtos));
        }
        
        return listOfStatItemDto;
    }
    
    public async Task<List<StatItemDto>> GetItemStatsDetails(string sku) {
        
        // Get List of items
        List<ItemStatsDetailsModel> listOfItemModel = await _context.ScanAsync<ItemStatsDetailsModel>(
            new [] {
                ItemStatsDetailsModel.BySKU(sku)
            }).GetRemainingAsync();

        // Convert list of items
        List<StatItemDto> listOfStatItemDto = new List<StatItemDto>();
        
        foreach (ItemStatsDetailsModel itemModel in listOfItemModel) {

            List<StatStockDto> listOfStatStockDtos = new List<StatStockDto>();
            
            if (itemModel.StockUpdates != null) {
                foreach (ItemStockUpdateModel.StockUpdateObject stockObject in itemModel.StockUpdates) {
                    StatStockDto statStockDto = new StatStockDto(stockObject.AmountChanged, stockObject.ReasonForChange,
                        stockObject.DateTimeAdded.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff"));
                    listOfStatStockDtos.Add(statStockDto);
                }
            }

            // Move this within the above if statement
            listOfStatItemDto.Add(
                new StatItemDto(
                    sku: itemModel.SKU,
                    businessId: itemModel.BusinessId,
                    category: itemModel.Category,
                    name: itemModel.Name,
                    stock: itemModel.TotalStock,
                    stockUpdates: listOfStatStockDtos));
        }
        
        return listOfStatItemDto;
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
            itemToSaveDto.TotalStock,
            itemToSaveDto.TotalOrders,
            itemToSaveDto.ImageFilename
        );
        await _context.SaveAsync(itemModel);

        ItemDto createdItemDto = new ItemDto(
            itemModel.SKU, 
            itemModel.BusinessId, 
            itemModel.Category,
            itemModel.Name,
            itemModel.GetTotalStock(),
            itemModel.GetTotalOrders(),
            itemModel.GetTotalStock() - itemModel.GetTotalOrders(),
            itemModel.ImageFilename
        );
        
        return createdItemDto;
    }
    
    public async Task<bool> IsNameInUse(CreateItemRequestDto createItemRequestDto) {
        var response = await _context.ScanAsync<Item>(
            new[] {
                Item.ByBusinessName(createItemRequestDto.Name)
            }).GetRemainingAsync();
        
        return response.Count > 0;
    }
    
    public async Task<bool> IsSkuInUse(string sku) {
        var response = await _context.ScanAsync<Item>(
            new[] {
                Item.ByItemSku(sku)
            }).GetRemainingAsync();
    
        return response.Count > 0;
    }

    public void Delete(DeleteItemDto deleteItemDto) {
        Item item = new Item(deleteItemDto.ItemId, deleteItemDto.BusinessId);
        _context.DeleteAsync(item);
    }
    
    public async Task<List<Dictionary<string, string>>> GetAllCategories(ValidateBusinessIdDto validateBusinessIdDto) {
        
        List<Item> listOfItemModel = await _context.ScanAsync<Item>(
            new [] {
                Item.ByBusinessId(validateBusinessIdDto.BusinessId)
            }).GetRemainingAsync();

        var categories = new HashSet<string>();
        var categoryList = new List<Dictionary<string, string>>();
        foreach (var item in listOfItemModel) {
            var category = item.Category;
            if (categories.Add(category)) {
                categoryList.Add(new Dictionary<string, string> {
                    {"Category", category}
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
            sku: itemToSaveDto.SKU, 
            businessId: itemToSaveDto.BusinessId,
            category: itemToSaveDto.Category,
            name: itemToSaveDto.Name,
            totalStock: itemToSaveDto.TotalStock, 
            totalOrders: itemToSaveDto.TotalOrders,
            imageFilename: itemToSaveDto.ImageFilename
        );
        await _context.SaveAsync(itemModel);
        
        // Return the updated Items details
        ItemDto updatedItemDto = new ItemDto(
            sku: itemModel.SKU, 
            businessId: itemModel.BusinessId, 
            category: itemModel.Category,
            name: itemModel.Name,
            totalStock: itemModel.GetTotalStock(),
            totalOrders: itemModel.GetTotalOrders(),
            availableStock: itemModel.GetTotalStock()-itemModel.GetTotalOrders(),
            imageFilename: itemModel.ImageFilename
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
        ItemDto itemDto = new ItemDto(item.SKU, item.BusinessId, item.Category, item.Name, item.GetTotalStock(), item.GetTotalOrders(), item.GetTotalStock() - item.GetTotalOrders(), item.ImageFilename);
        return itemDto;
    }
    
    public async Task<ItemOrderDto> SaveItemOrder(StoreItemOrderDto storeItemOrderDto) {
        // Validating details
        if (string.IsNullOrEmpty(storeItemOrderDto.BusinessId)) {
            throw new NullReferenceException("The stock update business ID cannot be null.");
        }
        if (string.IsNullOrEmpty(storeItemOrderDto.ItemSku)) {
            throw new NullReferenceException("The stock update item ID cannot be null.");
        }

        // Getting the existing items stock updates
        ItemOrdersModel existingItemOrders =
            await _context.LoadAsync<ItemOrdersModel>(storeItemOrderDto.ItemSku, storeItemOrderDto.BusinessId);

        // Adding to the existing stock updates
        existingItemOrders.AddItemOrderDetails(storeItemOrderDto.AmountOrdered, storeItemOrderDto.DateTimeAdded);

        // Saving all of the updates
        await _context.SaveAsync(existingItemOrders);

        // Returning the new object that was saved
        ItemOrderDto itemOrderDto =
            new ItemOrderDto(storeItemOrderDto.AmountOrdered, storeItemOrderDto.DateTimeAdded);
        
        return itemOrderDto;
    }

    public async Task<ItemConnectionsDto>? GetItemConnections(string businessId, string itemSku) {
        ItemConnectionsModel? existingItem = await _context.LoadAsync<ItemConnectionsModel>(itemSku, businessId);

        if (existingItem == null) {
            return null!;
        }
        
        return new ItemConnectionsDto(existingItem.Sku, existingItem.BusinessId, existingItem.Connections);
    }
    
    public async Task<ItemConnectionsDto> SaveItemConnections(ItemConnectionsDto itemConnectionsDto) {

        ItemConnectionsModel itemConnectionsModel = new ItemConnectionsModel(itemConnectionsDto.Sku,
            itemConnectionsDto.BusinessId, itemConnectionsDto.Connections);
        
        await _context.SaveAsync(itemConnectionsModel);
        
        return itemConnectionsDto;
    }

    public async Task<List<ItemConnectionsDto>> GetAllItemsWithConnections() {
        List<ItemConnectionsModel> listOfItemConnectionModels = await
            _context.ScanAsync<ItemConnectionsModel>(
                new [] {
                    // ItemConnectionsModel.ByBusinessId(businessId),
                    ItemConnectionsModel.ByValidConnection()
                }).GetRemainingAsync();

        List<ItemConnectionsDto> listOfItemConnectionsDtos = new List<ItemConnectionsDto>();

        foreach (ItemConnectionsModel connectionModel in listOfItemConnectionModels) {
            listOfItemConnectionsDtos.Add(new ItemConnectionsDto(connectionModel.Sku, connectionModel.BusinessId, connectionModel.Connections));
        }

        return listOfItemConnectionsDtos;
    }
}