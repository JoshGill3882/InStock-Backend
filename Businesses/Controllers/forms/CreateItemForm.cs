namespace instock_server_application.Businesses.Controllers.forms;

public class CreateItemForm
{
    public string SKU { get; }

    public string Category { get; }
    public string Name { get; }
    public string Stock { get; }
    
    public CreateItemForm(string sku, string category, string name, string stock)
    {
        SKU = sku;
        Category = category;
        Name = name;
        Stock = stock;
    }
}