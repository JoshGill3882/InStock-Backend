namespace instock_server_application.Businesses.Controllers.forms;

public class CreateItemOrderForm
{
    public string SKU { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public IFormFile? ImageFile { get; set; }
    
    public CreateItemOrderForm(string sku, string category, string name, int stock, IFormFile? imageFile)
    {
        SKU = sku;
        Category = category;
        Name = name;
        Stock = stock;
        ImageFile = imageFile;
    }

    public CreateItemOrderForm() {
        // Parameterless constructor required for model binding
    }
}