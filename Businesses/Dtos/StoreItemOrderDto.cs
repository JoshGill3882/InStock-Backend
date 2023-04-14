using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class StoreItemOrderDto {

    public string BusinessId { get; }
    public string ItemSku { get; }
    public int AmountOrdered { get; }
    public DateTime DateTimeAdded { get; }


    public StoreItemOrderDto(string businessId, string itemSku, int amountOrdered, DateTime dateTimeAdded) {
        BusinessId = businessId;
        ItemSku = itemSku;
        AmountOrdered = amountOrdered;
        DateTimeAdded = dateTimeAdded;
    }
}