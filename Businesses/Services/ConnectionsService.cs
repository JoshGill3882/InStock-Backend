using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class ConnectionsService {
    private readonly IBusinessRepository _businessRepository;

    public ConnectionsService(IBusinessRepository businessRepository) {
        _businessRepository = businessRepository;
    }
    
    public async Task<ConnectionDto> CreateStockUpdate(CreateStockUpdateRequestDto createStockUpdateRequestDto) {
        
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
        
        // Returning results
        // Return newly created stock update
        return stockUpdateDtoSaved;
    }
    
    
    //get all connections 
    
    //save new connection
    // Method to get all connections 
    
   // Method to add connection to users account 
   
   // Method to attempt sign in to mock shop
   // List of connection objects 
   // connection name (shop)
   // connection token
   
}