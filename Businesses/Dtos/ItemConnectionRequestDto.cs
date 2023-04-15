using instock_server_application.Shared.Dto;
using System;

namespace instock_server_application.Businesses.Dtos
{
    public class ItemConnectionRequestDto : DataTransferObjectSuperType
    {
        public string BusinessId { get; }
        public string ItemSku { get; }
        public string PlatformName { get; }
        public string PlatformItemSku { get; }

        public ItemConnectionRequestDto(ErrorNotification errorNotes) : base(errorNotes) {}

        public ItemConnectionRequestDto(string businessId, string itemSku, string platformName, string platformItemSku) {
            BusinessId = businessId;
            ItemSku = itemSku;
            PlatformName = platformName;
            PlatformItemSku = platformItemSku;
        }
    }
}