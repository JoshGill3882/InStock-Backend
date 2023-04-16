using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ItemDetailsDto : DataTransferObjectSuperType {
    public string SKU { get; }
    public string BusinessId { get; }
    public string Category { get; }
    public string Name { get; }
    public int TotalStock { get; }
    public int? AvailableStock { get; }
    public int TotalOrders { get; }
    public string? ImageFilename { get; } 
    
    public ItemDetailsDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ItemDetailsDto(string sku, string businessId, string category, string name, int totalStock, int totalOrders, int availableStock, string? imageFilename) {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        TotalStock = totalStock;
        TotalOrders = totalOrders;
        AvailableStock = availableStock;
        ImageFilename = imageFilename;
    }
}