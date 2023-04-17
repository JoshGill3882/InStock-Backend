using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ConnectedItemDetailsDto : DataTransferObjectSuperType {
    public string ShopName { get; }
    public string TotalStock { get; }
    public string AvailableStock { get; }
    public string TotalOrders { get; }
    public DateTime LastUpdated { get; }
    public string PlatformImageUrl { get; }


    public ConnectedItemDetailsDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ConnectedItemDetailsDto(string shopName, string totalStock, string availableStock, string totalOrders, DateTime lastUpdated, string platformImageUrl) {
        ShopName = shopName;
        TotalStock = totalStock;
        AvailableStock = availableStock;
        TotalOrders = totalOrders;
        LastUpdated = lastUpdated;
        PlatformImageUrl = platformImageUrl;
    }

}