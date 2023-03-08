using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ItemDto : DataTransferObjectSuperType {
    public string SKU { get; }
    public string Name { get; }
    public string Category { get; }
    public string Stock { get; }
    public string BusinessId { get; }


    public ItemDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ItemDto(string sku, string name, string category, string stock, string businessId)
    {
        SKU = sku;
        Name = name;
        Category = category;
        Stock = stock;
        BusinessId = businessId;
    }
}