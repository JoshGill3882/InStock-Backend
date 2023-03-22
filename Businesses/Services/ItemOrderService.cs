﻿using System.Numerics;
using System.Text.RegularExpressions;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Shared.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class ItemOrderService : IItemOrderService {
    private readonly IItemRepo _itemRepo;

    public ItemOrderService(IItemRepo itemRepo) {
        _itemRepo = itemRepo;
    }

    private void ValidateAmountChangeBy(ErrorNotification errorNotes, int amountOrdered, ItemDto existingItem) {
        const string errorKey = "amountOrdered";

        // long newStockLevel = (long) changeAmountBy + (long) existingItem.Stock;
        // if (newStockLevel > int.MaxValue) {
        //     errorNotes.AddError(errorKey, $"The stock level cannot be more than {int.MaxValue}.");
        // }
        // if (newStockLevel < int.MinValue) {
        //     errorNotes.AddError(errorKey, $"The stock level cannot be less than {int.MinValue}.");
        // }
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
        
        // Validate the amount we're updating
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

        // Update the Item's details with new stock level after the order
        int newStockLevel = existingItemDto.Stock - itemOrderDtoSaved.AmountOrdered;
        
        StoreItemDto updatedItemDto = new StoreItemDto(
            sku: existingItemDto.SKU, 
            businessId: existingItemDto.BusinessId, 
            category: existingItemDto.Category,
            name: existingItemDto.Name, 
            stock: newStockLevel);
        
        await _itemRepo.SaveExistingItem(updatedItemDto);
        
        // Returning results
        // Return newly created Item Update
        return itemOrderDtoSaved;
    }
}