using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Util.Services.Interfaces;
using Xunit.Sdk;

namespace instock_server_application.Businesses.Services; 

public class ItemOrderService : IItemOrderService {
    private readonly IItemRepo _itemRepo;
    private readonly INotificationService _notificationService;
    private readonly IMilestoneService _milestoneService;

    public ItemOrderService(IItemRepo itemRepo, INotificationService notificationService, IMilestoneService milestoneService) {
        _itemRepo = itemRepo;
        _notificationService = notificationService;
        _milestoneService = milestoneService;
    }

    private void ValidateAmountChangeBy(ErrorNotification errorNotes, int amountOrdered, ItemDto existingItem) {
        const string errorKey = "amountOrdered";

        if (existingItem.TotalOrders + amountOrdered < 0) {
            errorNotes.AddError(errorKey, "You can not have negative total orders.");
        }

        long newStockLevel = (long) existingItem.TotalOrders - (long) amountOrdered;

        if (newStockLevel > int.MaxValue) {
            errorNotes.AddError(errorKey, $"The stock level cannot be more than {int.MaxValue}.");
        }
        if (newStockLevel < int.MinValue) {
            errorNotes.AddError(errorKey, $"The stock level cannot be less than {int.MinValue}.");
        }
    }

    public async Task<ItemOrderDto> CreateItemOrder(CreateItemOrderRequestDto itemOrderRequestDto) {
    
        // Validation
        // Check if the user and business Ids are valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(itemOrderRequestDto.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(itemOrderRequestDto.UserBusinessId)) {
            throw new NullReferenceException("The BusinessId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(itemOrderRequestDto.ItemSku)) {
            throw new NullReferenceException("The ItemId cannot be null or empty.");
        }

        ErrorNotification errorNotes = new ErrorNotification();
        
        // Check if the user is allowed to edit the business, return as no need to do anymore validation
        if (!itemOrderRequestDto.UserBusinessId.Equals(itemOrderRequestDto.BusinessId)) {
            errorNotes.AddError(StockUpdateDto.USER_UNAUTHORISED_ERROR);
            return new ItemOrderDto(errorNotes);
        }
        
        // Check that the item exists, return as no need to do anymore validation
        ItemDto? existingItemDto =
            await _itemRepo.GetItem(itemOrderRequestDto.BusinessId, itemOrderRequestDto.ItemSku);
        
        if (existingItemDto == null) {
            errorNotes.AddError("This item does not exist");
            return new ItemOrderDto(errorNotes);
        }
        
        // Validate the amount ordered
        ValidateAmountChangeBy(errorNotes, itemOrderRequestDto.AmountOrdered, existingItemDto);

        // If we have had any errors then return them
        if (errorNotes.HasErrors) {
            return new ItemOrderDto(errorNotes);
        }
        
        // Saving to database
        // Create new Item Order record
        StoreItemOrderDto storeItemOrderDto = new StoreItemOrderDto(
            businessId: itemOrderRequestDto.BusinessId,
            itemSku: itemOrderRequestDto.ItemSku, 
            amountOrdered: itemOrderRequestDto.AmountOrdered,
            dateTimeAdded: DateTime.Now);
        
        ItemOrderDto itemOrderDtoSaved = await _itemRepo.SaveItemOrder(storeItemOrderDto);

        // Update the Item's details with new Total Orders level after the order
        int newTotalOrders = existingItemDto.TotalOrders + itemOrderDtoSaved.AmountOrdered;

        StoreItemDto updatedItemDto = new StoreItemDto(
            sku: existingItemDto.SKU,
            businessId: existingItemDto.BusinessId,
            category: existingItemDto.Category,
            name: existingItemDto.Name,
            totalStock: existingItemDto.TotalStock,
            totalOrders: newTotalOrders,
            imageFilename: existingItemDto.ImageFilename);
        
        await _itemRepo.SaveExistingItem(updatedItemDto);
        
        _notificationService.StockNotificationChecker(updatedItemDto);
        await _milestoneService.CheckMilestones(updatedItemDto);
        
        // Returning results
        // Return newly created Item Update
        return itemOrderDtoSaved;
    }

    public async void SetItemTotalOrders(string businessId, string itemSku, int newTotalOrders) {
        // Check that the item exists, return as no need to do anymore validation
        ItemDto? existingItemDto =
            await _itemRepo.GetItem(businessId, itemSku);
        
        if (existingItemDto == null) {
            throw new NotNullException();
        }

        ErrorNotification errorNotes = new ErrorNotification();
        
        // Work out the difference of the new total from the old
        int amountOrdered = newTotalOrders - existingItemDto.TotalOrders;

        // If nothing has changed then lets do nothing
        if (amountOrdered == 0) {
            return;
        }
        
        // Validate the amount ordered, if we have errors then do nothing
        ValidateAmountChangeBy(errorNotes, amountOrdered, existingItemDto);

        if (errorNotes.HasErrors) {
            return;
        }

        // Saving to database
        // Create new Item Order record
        StoreItemOrderDto storeItemOrderDto = new StoreItemOrderDto(
            businessId: businessId,
            itemSku: itemSku, 
            amountOrdered: amountOrdered,
            dateTimeAdded: DateTime.Now);
        
        ItemOrderDto itemOrderDtoSaved = await _itemRepo.SaveItemOrder(storeItemOrderDto);

        // Update the Item's details with new Total Orders
        StoreItemDto updatedItemDto = new StoreItemDto(
            sku: existingItemDto.SKU,
            businessId: existingItemDto.BusinessId,
            category: existingItemDto.Category,
            name: existingItemDto.Name,
            totalStock: existingItemDto.TotalStock,
            totalOrders: newTotalOrders,
            imageFilename: existingItemDto.ImageFilename);
        
        await _itemRepo.SaveExistingItem(updatedItemDto);
        
        _notificationService.StockNotificationChecker(updatedItemDto);
        await _milestoneService.CheckMilestones(updatedItemDto);
    }
}