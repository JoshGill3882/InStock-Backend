using System.Text.RegularExpressions;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Shared.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class ItemService : IItemService {
    private readonly IItemRepo _itemRepo;
    private readonly IUtilService _utilService;

    public ItemService(IItemRepo itemRepo, IUtilService utilService) {
        _itemRepo = itemRepo;
        _utilService = utilService;
    }
    
    private void ValidateItemName(ErrorNotification errorNotes, string itemName) {
        // Item Name Variables
        const string errorKey = "itemName";
        // This is quite a long max length but this is what Etsy Allows
        const int itemNameMaxLength = 140;
        // Based on Etsy recommendation for listings
        Regex itemNameRegex = new Regex(@"^[a-zA-Z0-9\s]*[@&]?[^$^`]$");

        if (string.IsNullOrEmpty(itemName)) {
            errorNotes.AddError(errorKey, "The item name cannot be empty.");
        }
        
        if (itemName.Length > itemNameMaxLength) {
            errorNotes.AddError(errorKey, $"The item name cannot exceed {itemNameMaxLength} characters.");
        }
        
        if (!itemNameRegex.IsMatch(itemName)) {
            errorNotes.AddError(errorKey, "The item name is invalid.");
        }
    }
    
    private void ValidateItemCategory(ErrorNotification errorNotes, string itemCategory) {
        // Item Name Variables
        const string errorKey = "itemCategory";
        const int itemCategoryMaxLength = 20;
        Regex itemCategoryRegex = new Regex(@"^[a-zA-Z0-9]+(\s+[a-zA-Z0-9]+)*$");

        if (string.IsNullOrEmpty(itemCategory)) {
            errorNotes.AddError(errorKey, "The item category cannot be empty.");
        }
        
        if (itemCategory.Length > itemCategoryMaxLength) {
            errorNotes.AddError(errorKey, $"The item category cannot exceed {itemCategoryMaxLength} characters.");
        }
        
        if (!itemCategoryRegex.IsMatch(itemCategory)) {
            errorNotes.AddError(errorKey, "The item category is invalid.");
        }
    }
    
    private async Task ValidateDuplicateName(ErrorNotification errorNotes, CreateItemRequestDto newItemRequestDto){
        const string errorKey = "duplicateItemName";
        var isDuplicate = await _itemRepo.IsNameInUse(newItemRequestDto);
        if (isDuplicate)
        {
            errorNotes.AddError(errorKey, "You already have an item with that name");
        }
    }

    private async Task ValidateDuplicateSKU(ErrorNotification errorNotes, CreateItemRequestDto newItemRequestDto)
    {
        const string errorKey = "duplicateSKU";
        var isDuplicate = await _itemRepo.IsSKUInUse(newItemRequestDto.SKU, newItemRequestDto.BusinessId);
        if (isDuplicate)
        {
            errorNotes.AddError(errorKey, "You already have an item with that SKU");
        }
    }

    public async Task<List<Dictionary<string, string>>?> GetItems(UserDto userDto, string businessId) {
        
        if (_utilService.CheckUserBusinessId(userDto.UserBusinessId, businessId)) {
            List<Dictionary<string, AttributeValue>> responseItems = _itemRepo.GetAllItems(businessId).Result;
            List<Dictionary<string, string>> items = new();

            // User has access, but incorrect businessID or no items found
            if (responseItems.Count == 0) {
                // Return an empty list
                return items;
            }

            foreach (Dictionary<string, AttributeValue> item in responseItems) {
                string stock = item["Stock"].S ?? item["Stock"].N;
                items.Add(
                    new () {
                        {"SKU", item["SKU"].S},
                        {"BusinessId", item["BusinessId"].S},
                        {"Category", item["Category"].S},
                        {"Name", item["Name"].S},
                        {"Stock", stock}
                    }
                );
            }

            return items;
        }

        // If the user doesn't have access, return "null"
        return null;
    }
    
    public async Task<List<Dictionary<string, string>>?> GetItemsWithUpdates(UserDto userDto, string businessId) {
        
        if (_utilService.CheckUserBusinessId(userDto.UserBusinessId, businessId)) {
            List<Dictionary<string, AttributeValue>> responseItems = _itemRepo.GetAllItems(businessId).Result;
            List<Dictionary<string, string>> items = new();

            // User has access, but incorrect businessID or no items found
            if (responseItems.Count == 0) {
                // Return an empty list
                return items;
            }

            foreach (Dictionary<string, AttributeValue> item in responseItems) {
                if (item.ContainsKey("StockUpdates"))
                {
                    string stock = item["Stock"].S ?? item["Stock"].N;
                    items.Add(
                        new () {
                            {"SKU", item["SKU"].S},
                            {"BusinessId", item["BusinessId"].S},
                            {"Category", item["Category"].S},
                            {"Name", item["Name"].S},
                            {"Stock", stock},
                            {"StockUpdates", item["StockUpdates"].S}
                        }
                    );
                }

            }

            return items;
        }

        // If the user doesn't have access, return "null"
        return null;
    }
    
    public async Task<ItemDto> CreateItem(CreateItemRequestDto newItemRequestDto) {

        ErrorNotification errorNotes = new ErrorNotification();
        
        // Check if the user Id is valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(newItemRequestDto.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        
        // Validate the Item details
        ValidateItemName(errorNotes, newItemRequestDto.Name);
        ValidateItemCategory(errorNotes, newItemRequestDto.Category);
        await ValidateDuplicateName(errorNotes, newItemRequestDto);
        await ValidateDuplicateSKU(errorNotes, newItemRequestDto);
        
        // If we've got errors then return the notes and not make a repo call
        if (errorNotes.HasErrors) {
            return new ItemDto(errorNotes);
        }
        
        // Calling repo to create the business for the user
        StoreItemDto itemToSaveDto =
            new StoreItemDto(newItemRequestDto.SKU, newItemRequestDto.BusinessId, newItemRequestDto.Category, 
                newItemRequestDto.Name, newItemRequestDto.Stock);
        
        ItemDto createdItem = await _itemRepo.SaveNewItem(itemToSaveDto);

        return createdItem;
    }

    public async Task<DeleteItemDto> DeleteItem(DeleteItemDto deleteItemDto) {
        if (_utilService.CheckUserBusinessId(deleteItemDto.UserBusinessId, deleteItemDto.BusinessId)) {
            // Delete the item
            _itemRepo.Delete(deleteItemDto);
            // Return string response
            return deleteItemDto;
        }

        ErrorNotification errorNotification = new ErrorNotification();
        errorNotification.AddError(DeleteItemDto.USER_UNAUTHORISED_ERROR);
        return new DeleteItemDto(errorNotification);
    }

    public async Task<List<Dictionary<string, string>>?> GetCategories(ValidateBusinessIdDto validateBusinessIdDto) {

        if (_utilService.CheckUserBusinessId(validateBusinessIdDto.UserBusinessId, validateBusinessIdDto.BusinessId)) {
            List<Dictionary<string, AttributeValue>> responseCategories = _itemRepo.GetAllCategories(validateBusinessIdDto).Result;
            List<Dictionary<string, string>> categories = new();

            // User has access, but incorrect businessID or no items found
            if (responseCategories.Count == 0) {
                // Return an empty list
                return categories;
            }

            foreach (Dictionary<string, AttributeValue> category in responseCategories) {
                categories.Add(
                    new() {
                        { "Category", category["Category"].S }
                    }
                );
            }
            return categories;
        }
        
        // If the user doesn't have access, return "null"
        return null;
    }
    
    public async Task<StockUpdateDto> CreateStockUpdate(CreateStockUpdateRequestDto createStockUpdateRequestDto) {
        
        // Validation
        // Check if the user and business Ids are valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(createStockUpdateRequestDto.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(createStockUpdateRequestDto.UserBusinessId)) {
            throw new NullReferenceException("The BusinessId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(createStockUpdateRequestDto.ItemSku)) {
            throw new NullReferenceException("The ItemId cannot be null or empty.");
        }

        ErrorNotification errorNotes = new ErrorNotification();
        
        // Check if the user is allowed to edit the business, return as no need to do anymore validation
        if (!createStockUpdateRequestDto.UserBusinessId.Equals(createStockUpdateRequestDto.BusinessId)) {
            errorNotes.AddError(StockUpdateDto.USER_UNAUTHORISED_ERROR);
            return new StockUpdateDto(errorNotes);
        }
        
        // Check that the item exists
        ItemDto? existingItemDto =
            await _itemRepo.GetItem(createStockUpdateRequestDto.BusinessId, createStockUpdateRequestDto.ItemSku);
        
        if (existingItemDto == null) {
            errorNotes.AddError("This item does not exist");
        }

        // If we have had any errors then return this
        if (errorNotes.HasErrors) {
            return new StockUpdateDto(errorNotes);
        }
        
        // Saving to database
        // Create new Stock Update record
        StoreStockUpdateDto stockUpdateDtoToSave = new StoreStockUpdateDto(
            businessId: createStockUpdateRequestDto.BusinessId,
            itemSku: createStockUpdateRequestDto.ItemSku, 
            changeStockAmountBy: createStockUpdateRequestDto.ChangeStockAmountBy,
            reasonForChange: createStockUpdateRequestDto.ReasonForChange, 
            dateTimeAdded: DateTime.Now);
        
        StockUpdateDto stockUpdateDtoSaved = await _itemRepo.SaveStockUpdate(stockUpdateDtoToSave);

        // Update the Item's details with new stock level after the update
        int newStockLevel = existingItemDto.Stock + stockUpdateDtoSaved.ChangeStockAmountBy;
        
        StoreItemDto updatedItemDto = new StoreItemDto(
            sku: existingItemDto.SKU, 
            businessId: existingItemDto.BusinessId, 
            category: existingItemDto.Category,
            name: existingItemDto.Name, 
            stock: newStockLevel);
        
        await _itemRepo.SaveExistingItem(updatedItemDto);
        
        // Returning results
        // Return newly created stock update
        return stockUpdateDtoSaved;
    }
}