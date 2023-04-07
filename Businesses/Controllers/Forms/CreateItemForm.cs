namespace instock_server_application.Businesses.Controllers.forms;

public class CreateItemForm
{
    public string SKU { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public IFormFile? File { get; set; }
    
    public CreateItemForm(string sku, string category, string name, int stock, IFormFile? file)
    {
        SKU = sku;
        Category = category;
        Name = name;
        Stock = stock;
        File = file;
    }

    public CreateItemForm()
    {
        // Parameterless constructor required for model binding
    }
}