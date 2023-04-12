﻿using System.Security.Claims;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Util.Dto;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
[Route("/businesses/{businessId}")]
public class ItemController : ControllerBase {
    private readonly IItemService _itemService;
    
    public ItemController(IItemService itemService) {
        _itemService = itemService;
    }

    /// <summary>
    /// Function for getting all the items for a specific business, providing the currently logged in user has access
    /// </summary>
    /// <param name="businessIdModel"> The BusinessID to get all the items for </param>
    /// <returns> List of all the Items found, or an error message with a 404 status code </returns>
    [HttpGet]
    [Route("items")]
    public async Task<IActionResult> GetAllItems([FromRoute] string businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }
        
        // Creating new userDto to pass into service
        UserDto currentUserDto = new UserDto(currentUserId, currentUserBusinessId);
        
        List<Dictionary<string, string>>? items = _itemService.GetItems(currentUserDto, businessId).Result;

        if (items == null) {
            return Unauthorized();
        } if (items.Count == 0) {
            return NotFound();
        }

        return Ok(items);
    }

    /// <summary>
    /// Add item for a specific business
    /// </summary>
    /// <param name="newItemForm"> Create item form </param>
    /// <param name="businessId"> Unique ID for the business the item needs to be added to</param>
    /// <returns> Item created, or error with relevant status code</returns>
    [HttpPost]
    [Route("items")]
    public async Task<IActionResult> CreateItem([FromForm] CreateItemForm newItemForm, [FromRoute] string businessId) {

        // Get our current UserId and BusinessId to validate and pass to the items service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        
        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId)) {
            return Unauthorized();
        }

        // Creating CreateItemDTO to pass the details to the service for processing
        CreateItemRequestDto createItemRequestDto = new CreateItemRequestDto(
            newItemForm.SKU,
            businessId, 
            newItemForm.Category, 
            newItemForm.Name, 
            newItemForm.Stock, 
            currentUserId,
            newItemForm.ImageFile
        );

        // Attempting to create new item
        ItemDto createdItemDto = await _itemService.CreateItem(createItemRequestDto);

        // If errors then return 401 with the error messages
        if (createdItemDto.ErrorNotification.HasErrors) {
            return new BadRequestObjectResult(createdItemDto.ErrorNotification);
        }
        
        // If not errors then return 201 with the URI and newly created object details
        string? createdItemUrl = Url.Action(controller: "item", action: nameof(GetItem), values:new
        {
            businesses=Url.RouteUrl("businesses"),
            businessId = createdItemDto.BusinessId,
            items=Url.RouteUrl("items"),
            itemId=createdItemDto.SKU
        }, protocol:Request.Scheme);
        
        return Created(createdItemUrl ?? string.Empty, new {
            sku = createdItemDto.SKU,
            businessId = createdItemDto.BusinessId,
            category = createdItemDto.Category,
            name = createdItemDto.Name,
            stock = createdItemDto.Stock,
            imageUrl = createdItemDto.ImageUrl
        });
    }
    
    [HttpGet]
    [Route("items/{itemId}")]
    public async Task<IActionResult> GetItem([FromRoute] string itemId, string businessId) {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [Route("items/{itemId}")]
    public async Task<IActionResult> DeleteItem([FromRoute] string itemId, [FromRoute] string businessId) {
        DeleteItemDto result = _itemService.DeleteItem(new DeleteItemDto(itemId, User.FindFirstValue("BusinessId"), businessId)).Result;

        if (result.ErrorNotification.HasErrors) {
            if (result.ErrorNotification.Errors["otherErrors"].Contains(DeleteItemDto.USER_UNAUTHORISED_ERROR)) {
                return Unauthorized();
            }
            return new BadRequestObjectResult(result.ErrorNotification.Errors);
        }
        return Ok();
    }
    
    /// <summary>
    /// Function for getting all the categories for a specific business, providing the currently logged in user has access
    /// </summary>
    /// <param name="businessIdModel"> The BusinessID to get all the categories for </param>
    /// <returns> List of all the Categories found, or an error message with a 404 status code </returns>
    [HttpGet]
    [Route("categories")]
    public async Task<IActionResult> GetAllCategories([FromRoute] string businessId) {
        
        // Get our current UserId and BusinessId to validate and pass to the business service
        string? currentUserId = User.FindFirstValue("Id") ?? null;
        string? currentUserBusinessId = User.FindFirstValue("BusinessId") ?? null;

        // Check there are no issues with the userId
        if (string.IsNullOrEmpty(currentUserId) | string.IsNullOrEmpty(currentUserBusinessId)) {
            return Unauthorized();
        }

        List<Dictionary<string, string>>? categories = _itemService.GetCategories(new ValidateBusinessIdDto(currentUserBusinessId, businessId)).Result;

        if (categories == null) {
            return Unauthorized();
        } if (categories.Count == 0) {
            return NotFound();
        }

        return Ok(categories);
    }
}