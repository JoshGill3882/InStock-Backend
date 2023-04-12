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
                    { "SKU", item.SKU },
                    { "BusinessId", item.BusinessId },
                    { "Category", item.Category },
                    { "Name", item.Name },
                    { "Stock", item.Stock.ToString() },
                    { "TotalStock", item.TotalStock.ToString() }
                });
        }

        return listOfItems;
    }
}