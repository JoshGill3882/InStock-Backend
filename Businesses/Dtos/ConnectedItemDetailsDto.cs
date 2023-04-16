using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ConnectedItemDetailsDto : DataTransferObjectSuperType {
    public string ShopName { get; }
    public string TotalStock { get; }
    public string AvailableStock { get; }
    public string TotalOrders { get; }
    public int LastUpdated { get; }
    
    public Dictionary<string, Dictionary<string, string>> ConnectedItems { get; }

    public ConnectedItemDetailsDto(ErrorNotification errorNotes) : base(errorNotes) {}

    public ConnectedItemDetailsDto(string shopName, string totalStock, string availableStock, string totalOrders, Dictionary<string, Dictionary<string, string>> connectedItems, int lastUpdated) {
        ShopName = shopName;
        TotalStock = totalStock;
        AvailableStock = availableStock;
        TotalOrders = totalOrders;
        ConnectedItems = connectedItems;
        LastUpdated = lastUpdated;
    }

}