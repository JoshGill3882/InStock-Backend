namespace instock_server_application.Businesses.Dtos;

public class CreateItemRequestDto
{
    public string SKU { get; }
    public string BusinessId { get; }
    public string Category { get; }
    public string Name { get; }
    public int Stock { get; }
    public string UserId { get; }

    public CreateItemRequestDto(string sku, string businessId, string category, string name, int stock, string userId)
    {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        Stock = stock;
        UserId = userId;
    }
}