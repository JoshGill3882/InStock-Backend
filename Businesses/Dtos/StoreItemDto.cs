namespace instock_server_application.Businesses.Dtos;

public class StoreItemDto
{
    public string SKU { get; }
    public string BusinessId { get; }
    public string Category { get; }
    public string Name { get; }
    public int TotalStock { get; }
    public int TotalOrders  { get; }
    public string? ImageFilename { get; }

    public StoreItemDto(string sku, string businessId, string category, string name, int totalStock, int totalOrders, string? imageFilename) {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        TotalStock = totalStock;
        TotalOrders = totalOrders;
        ImageFilename = imageFilename;
    }
}