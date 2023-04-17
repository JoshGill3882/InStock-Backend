using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ItemDetailsDto : DataTransferObjectSuperType {
    public string Sku { get; }
    public string Name { get; }
    public int TotalStock { get; }
    public int? AvailableStock { get; }
    public int TotalOrders { get; }
    public int TotalSales { get; }
    public string ImageUrl { get; }
    public List<ConnectedItemDetailsDto> ConnectedItems { get; }

    public ItemDetailsDto(ErrorNotification errorNotes) : base(errorNotes) {}
    
    public ItemDetailsDto(string sku, string name, int totalStock, int? availableStock, int totalOrders, string imageUrl, List<ConnectedItemDetailsDto> connectedItems, int totalSales) {
        Sku = sku;
        Name = name;
        TotalStock = totalStock;
        AvailableStock = availableStock;
        TotalOrders = totalOrders;
        ImageUrl = imageUrl;
        ConnectedItems = connectedItems;
        TotalSales = totalSales;
    }
}