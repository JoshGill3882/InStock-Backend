using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class StatItemDto : DataTransferObjectSuperType {
    public string SKU { get; }
    public string BusinessId { get; }
    public string Category { get; }
    public string Name { get; }
    public string Stock { get; }
    public List<StatStockDto> StockUpdates { get; }
    
    public StatItemDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public StatItemDto(string sku, string businessId, string category, string name, string stock, List<StatStockDto> stockUpdates)
    {
        SKU = sku;
        BusinessId = businessId;
        Category = category;
        Name = name;
        Stock = stock;
        StockUpdates = stockUpdates;
    }
}