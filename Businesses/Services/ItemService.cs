using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public class ItemService : IItemService {
    private readonly IItemRepo _itemRepo;
    private readonly IBusinessService _businessService;

    public ItemService(IItemRepo itemRepo, IBusinessService businessService) {
        _itemRepo = itemRepo;
        _businessService = businessService;
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
        
        // Check if the user has already got a business, this makes the following other validations meaningless so return
        // if (await _businessRepository.DoesUserOwnABusiness(new Guid(newBusinessRequest.UserId))) {
        //     errorNotes.AddError("A Business is already associated with your account.");
        //     return new BusinessDto(errorNotes);
        // }
        
        // Validate and clean the Business Name
        // ValidateBusinessName(errorNotes, newBusinessRequest.BusinessName);

        // If we've got errors then return the notes and not make a repo call
        // if (errorNotes.HasErrors) {
        //     return new ItemDto(errorNotes);
        // }
        
        // Calling repo to create the business for the user
        StoreItemDto itemToSaveDto =
            new StoreItemDto(newItemRequestDto.SKU, newItemRequestDto.BusinessId, newItemRequestDto.Category, newItemRequestDto.Name, newItemRequestDto.Stock);
        
        ItemDto createdItem = await _itemRepo.SaveNewItem(itemToSaveDto);

        return createdItem;
    }
}