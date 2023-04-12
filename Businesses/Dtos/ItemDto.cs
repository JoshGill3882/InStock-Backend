using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ItemDto : DataTransferObjectSuperType {
    public string SKU { get; }
    public string BusinessId { get; }
    public string Category { get; }
    public string Name { get; }
    public int Stock { get; }    // TODO Replaced by TotalStock but remains for backward compatability
    public int TotalStock { get; }
    public int? AvailableStock { get; }
    public int TotalOrders { get; }
    
    public ItemDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ItemDto(string sku, string businessId, string category, string name, int totalStock, int totalOrders, int availableStock)
    {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        Stock = totalStock;
        TotalStock = totalStock;
        TotalOrders = totalOrders;
        AvailableStock = availableStock;
    }
}