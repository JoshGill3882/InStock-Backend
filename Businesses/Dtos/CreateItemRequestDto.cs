namespace instock_server_application.Businesses.Dtos;

public class CreateItemRequestDto
{
    public string SKU { get; }
    public string BusinessId { get; }
    public string Category { get; }
    public string Name { get; }
    public string Stock { get; }
    public string UserId { get; }

    public CreateItemRequestDto(string sku, string businessId, string category, string name, string stock, string userId)
    {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        Stock = stock;
        UserId = userId;
    }
}