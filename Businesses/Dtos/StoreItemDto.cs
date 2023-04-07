namespace instock_server_application.Businesses.Dtos;

public class StoreItemDto
{
    public string SKU { get; }
    public string BusinessId { get; }
    public string Category { get; }
    public string Name { get; }
    public int Stock { get; }
    public string? ImageUrl { get; }

    public StoreItemDto(string sku, string businessId, string category, string name, int stock, string? imageUrl)
    {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        Stock = stock;
        ImageUrl = imageUrl;
    }
}