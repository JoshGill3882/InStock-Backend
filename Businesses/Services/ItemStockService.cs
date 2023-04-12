using System.Text.RegularExpressions;
using FirebaseAdmin.Messaging;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class ItemStockService : IItemStockService {
    private readonly IItemRepo _itemRepo;
    private readonly IBusinessRepository _businessRepository;

    public ItemStockService(IItemRepo itemRepo, IBusinessRepository businessRepository) {
        _itemRepo = itemRepo;
        _businessRepository = businessRepository;
    }

    private void ValidateAmountChangeBy(ErrorNotification errorNotes, int changeAmountBy, ItemDto existingItem) {
        const string errorKey = "ChangeAmountBy";

        long newStockLevel = (long) changeAmountBy + (long) existingItem.Stock;
        if (newStockLevel > int.MaxValue) {
            errorNotes.AddError(errorKey, $"The stock level cannot be more than {int.MaxValue}.");
        }
        if (newStockLevel < 0) {
            errorNotes.AddError(errorKey, "The stock level cannot be less than 0.");
        }
    }    
    private void ValidateReasonForChange(ErrorNotification errorNotes, string reasonForChange) {
        const string errorKey = "ReasonForChange";
        const int maxLength = 250;
        const string permittedRegex = @"[a-zA-Z0-9,.\-& ]";

        if (reasonForChange.Length > maxLength) {
            errorNotes.AddError(errorKey, $"The reason cannot exceed {maxLength}.");
        }

        if (!Regex.IsMatch(reasonForChange, permittedRegex)) {
            errorNotes.AddError(errorKey, "The reason can only contain alphanumeric characters, spaces, commas, and dashes.");
        }
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
        
        // Check that the item exists, return as no need to do anymore validation
        ItemDto? existingItemDto =
            await _itemRepo.GetItem(createStockUpdateRequestDto.BusinessId, createStockUpdateRequestDto.ItemSku);
        
        if (existingItemDto == null) {
            errorNotes.AddError("This item does not exist");
            return new StockUpdateDto(errorNotes);
        }
        
        // Validate the amount we're updating
        ValidateAmountChangeBy(errorNotes, createStockUpdateRequestDto.ChangeStockAmountBy, existingItemDto);

        // Validate the update's reason text
        ValidateReasonForChange(errorNotes, createStockUpdateRequestDto.ReasonForChange);
        
        // If we have had any errors then return them
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
            stock: newStockLevel,
            imageUrl: existingItemDto.ImageUrl
        );
        
        await _itemRepo.SaveExistingItem(updatedItemDto);

        switch (newStockLevel) {
            case 5:
                LowStockNotification(updatedItemDto);
                break;
            case 0:
                NoStockNotification(updatedItemDto);
                break;
        }
        
        // Returning results
        // Return newly created stock update
        return stockUpdateDtoSaved;
    }

    private void LowStockNotification(StoreItemDto itemDto) {
        var message = new MulticastMessage() {
            Notification = new Notification() {
                Title = "Low Stock",
                Body = "You are low on: " + itemDto.Name
            },
            Data = new Dictionary<string, string>() {
                { "SKU", itemDto.SKU },
                { "BusinessId", itemDto.BusinessId },
                { "Category", itemDto.Category },
                { "Name", itemDto.Name },
                { "Stock", itemDto.Stock.ToString() }
            },
            Tokens = GetClientDeviceTokens(itemDto.BusinessId)
        };
        FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
    }

    private void NoStockNotification(StoreItemDto itemDto) {
        var message = new MulticastMessage() {
            Notification = new Notification() {
                Title = "Out of Stock",
                Body = "You are out of Stock on: " + itemDto.Name
            },
            Data = new Dictionary<string, string>() {
                { "SKU", itemDto.SKU },
                { "BusinessId", itemDto.BusinessId },
                { "Category", itemDto.Category },
                { "Name", itemDto.Name },
                { "Stock", itemDto.Stock.ToString() }
            },
            Tokens = GetClientDeviceTokens(itemDto.BusinessId)
        };
        FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
    }

    private List<string> GetClientDeviceTokens(string businessId) {
        BusinessDto businessDto = _businessRepository.GetBusiness(new(null, businessId)).Result;
        return businessDto!.DeviceKeys;
    }
}