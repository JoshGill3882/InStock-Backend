using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ListOfItemDto : DataTransferObjectSuperType {

    public List<ItemDto> ListOfItems;
    
    public static string ERROR_UNAUTHORISED = "Unauthorised";

    public ListOfItemDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ListOfItemDto(List<ItemDto> listOfItems) {
        ListOfItems = listOfItems;
    }

    public List<Dictionary<string, object>> ToDictionaryForHttpResponse() {
        var listOfItems = new List<Dictionary<string, object>>();
        
        foreach (ItemDto item in ListOfItems) {
            listOfItems.Add(
                new Dictionary<string, object>(){
                    { nameof(ItemDto.SKU), item.SKU },
                    { nameof(ItemDto.BusinessId), item.BusinessId },
                    { nameof(ItemDto.Category), item.Category },
                    { nameof(ItemDto.Name), item.Name },
                    { "Stock", item.Stock.ToString() },
                    { nameof(ItemDto.TotalStock), item.TotalStock.ToString() },
                    { nameof(ItemDto.TotalOrders), item.TotalOrders.ToString() },
                    { nameof(ItemDto.ImageFilename), item.ImageFilename ?? "" },
                });
        }

        return listOfItems;
    }
}