using System.Text.RegularExpressions;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace instock_server_application.Businesses.Services; 

public class ItemService : IItemService {
    private readonly IItemRepo _itemRepo;
    private readonly IBusinessService _businessService;

    public ItemService(IItemRepo itemRepo, IBusinessService businessService) {
        _itemRepo = itemRepo;
        _businessService = businessService;
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
    
    private void ValidateItemStock(ErrorNotification errorNotes, string itemStock) {
        // Item Name Variables
        const string errorKey = "itemStock";
        
        if (string.IsNullOrEmpty(itemStock)) {
            errorNotes.AddError(errorKey, "The item name cannot be empty.");
        }

        if (!int.TryParse(itemStock, out int n))
        {
            errorNotes.AddError(errorKey, "The item Stock level must be a number.");

        }
    }

    private async Task ValidateDuplicateName(ErrorNotification errorNotes, string businessId, string itemName)
    {
        const string errorKey = "duplicateItemName";
        var isDuplicate = await _itemRepo.IsNameInUse(businessId, itemName);
        if (isDuplicate)
        {
            errorNotes.AddError(errorKey, "You already have an item with that name");
        }
    }
    
    private async Task ValidateDuplicateSKU(ErrorNotification errorNotes, string businessId, string itemSku)
    {
        const string errorKey = "duplicateSKU";
        var isDuplicate = await _itemRepo.IsSKUInUse(itemSku, businessId);
        if (isDuplicate)
        {
            errorNotes.AddError(errorKey, "You already have an item with that SKU");
        }
    }

    public async Task<List<Dictionary<string, string>>?> GetItems(UserDto userDto, string businessId) {
        
        if (_businessService.CheckBusinessIdInJwt(userDto, businessId)) {
            List<Dictionary<string, AttributeValue>> responseItems = _itemRepo.GetAllItems(businessId).Result;
            List<Dictionary<string, string>> items = new();

            // User has access, but incorrect businessID or no items found
            if (responseItems.Count == 0) {
                // Return an empty list
                return items;
            }

            foreach (Dictionary<string, AttributeValue> item in responseItems) {
                items.Add(
                    new () {
                        {"SKU", item["SKU"].S},
                        {"BusinessId", item["BusinessId"].S},
                        {"Category", item["Category"].S},
                        {"Name", item["Name"].S},
                        {"Stock", item["Stock"].N}
                    }
                );
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
        ValidateItemStock(errorNotes, newItemRequestDto.Stock);
        await ValidateDuplicateName(errorNotes, newItemRequestDto.BusinessId, newItemRequestDto.Name);
        await ValidateDuplicateSKU(errorNotes, newItemRequestDto.BusinessId, newItemRequestDto.SKU);

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

    public async Task<ItemDto> UpdateItem(UpdateItemRequestDto updateItemRequestDto) {
        
        // Check if the user Id is valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(updateItemRequestDto.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }

        ErrorNotification errorNotes = new ErrorNotification();

        // Getting the existing Item and apply change
        ItemDto updatedItem = await _itemRepo.GetItemWithUpdate(updateItemRequestDto.ItemId, updateItemRequestDto.UserBusinessId, updateItemRequestDto.ItemPatchDocument);

        if (updatedItem.ErrorNotification.HasErrors) {
            return updatedItem;
        }
        
        // Validate the Item details
        ValidateItemName(errorNotes, updatedItem.Name);
        ValidateItemCategory(errorNotes, updatedItem.Category);
        ValidateItemStock(errorNotes, updatedItem.Stock);
        await ValidateDuplicateName(errorNotes, updatedItem.BusinessId, updatedItem.Name);
        await ValidateDuplicateSKU(errorNotes, updatedItem.BusinessId, updatedItem.SKU);
        
        // If we've got errors then return the notes and not make a repo call
        if (errorNotes.HasErrors) {
            return new ItemDto(errorNotes);
        }

        StoreItemDto itemToSaveDto = new StoreItemDto(updatedItem.SKU, updatedItem.BusinessId, updatedItem.Category,
            updatedItem.Name, updatedItem.Stock);

        ItemDto savedItem = await _itemRepo.SaveExistingItem(itemToSaveDto);

        return savedItem;
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
        if (string.IsNullOrEmpty(createStockUpdateRequestDto.ItemId)) {
            throw new NullReferenceException("The ItemId cannot be null or empty.");
        }

        ErrorNotification errorNotes = new ErrorNotification();
        
        // Check if the user is allowed to edit the business, return as no need to do anymore validation
        if (!createStockUpdateRequestDto.UserBusinessId.Equals(createStockUpdateRequestDto.BusinessId)) {
            errorNotes.AddError("You are not authorised to update this business.");
            return new StockUpdateDto(errorNotes);
        }
        
        // Check that the item exists
        ItemDto? existingItemDto =
            await _itemRepo.GetItem(createStockUpdateRequestDto.BusinessId, createStockUpdateRequestDto.ItemId);
        
        if (existingItemDto == null) {
            errorNotes.AddError("This item does not exist");
        }

        if (errorNotes.HasErrors) {
            return new StockUpdateDto(errorNotes);
        }
        
        // Saving to database
        // Create new Stock Update record
        StoreStockUpdateDto stockUpdateDtoToSave = new StoreStockUpdateDto(createStockUpdateRequestDto.BusinessId,
            createStockUpdateRequestDto.ItemId, createStockUpdateRequestDto.ChangeStockAmountBy,
            createStockUpdateRequestDto.ReasonForChange, DateTime.Now);
        StockUpdateDto stockUpdateDtoSaved = await _itemRepo.SaveStockUpdate(stockUpdateDtoToSave);

        // Update Item details with new stock level
        int newStockLevel = int.Parse(existingItemDto.Stock) + createStockUpdateRequestDto.ChangeStockAmountBy;
        StoreItemDto updatedItemDto = new StoreItemDto(existingItemDto.SKU, existingItemDto.BusinessId, existingItemDto.Category,
            existingItemDto.Name, newStockLevel.ToString());
        await _itemRepo.SaveExistingItem(updatedItemDto);
        
        // Returning results
        // Return newly created stock update 
        return stockUpdateDtoSaved;
    }
}