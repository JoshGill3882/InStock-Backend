using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ItemDetailsDto : DataTransferObjectSuperType {
    public string Sku { get; }
    public string Name { get; }
    public int TotalStock { get; }
    public int? AvailableStock { get; }
    public int TotalOrders { get; }
    public string? ImageFilename { get; }
    public List<ConnectedItemDetailsDto> ConnectedItems { get; }

    public ItemDetailsDto(ErrorNotification errorNotes) : base(errorNotes) {}
    
    public ItemDetailsDto(string sku, string name, int totalStock, int? availableStock, int totalOrders, string? imageFilename, List<ConnectedItemDetailsDto> connectedItems) {
        Sku = sku;
        Name = name;
        TotalStock = totalStock;
        AvailableStock = availableStock;
        TotalOrders = totalOrders;
        ImageFilename = imageFilename;
        ConnectedItems = connectedItems;
    }

    public ItemDetailsDto(ItemDto itemDto, List<ConnectedItemDetailsDto> connectedItemDetailsDtos) {
        Sku = itemDto.SKU;
        Name = itemDto.Name;
        TotalStock = itemDto.TotalStock;
        AvailableStock = itemDto.AvailableStock;
        TotalOrders = itemDto.TotalOrders;
        ImageFilename = itemDto.ImageFilename;
        ConnectedItems = connectedItemDetailsDtos;
    }
}