using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos;

public class ItemRequestDto : DataTransferObjectSuperType {
    public string Sku { get; }
    public string BusinessId { get; }

    public ItemRequestDto(ErrorNotification errorNotes) : base(errorNotes) {}

    public ItemRequestDto(string sku, string businessId) {
        Sku = sku;
        BusinessId = businessId;
    }
}