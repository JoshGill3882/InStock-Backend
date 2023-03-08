using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ItemDto : DataTransferObjectSuperType {
    public string SKU { get; }
    public string BusinessId { get; }
    public string Category { get; }
    public string Name { get; }
    public string Stock { get; }
    


    public ItemDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ItemDto(string sku, string businessId, string category, string name, string stock)
    {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        Stock = stock;
    }
}