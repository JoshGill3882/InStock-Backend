using FirebaseAdmin.Messaging;
using instock_server_application.AwsS3.Services.Interfaces;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Util.Dto;
using instock_server_application.Util.Services.Interfaces;

namespace instock_server_application.Util.Services; 

public class NotificationService : INotificationService {
    private readonly IBusinessRepository _businessRepository;
    private readonly IStorageService _storageService;

    public NotificationService(IBusinessRepository businessRepository, IStorageService storageService) {
        _businessRepository = businessRepository;
        _storageService = storageService;
    }

    public NotificationService() {
    }

    public void StockNotificationChecker(StoreItemDto itemDto) {
        int availableStock = itemDto.TotalStock - itemDto.TotalOrders;
        if (availableStock <= 5 && availableStock > 0)
            SendNotification(CreateNotification("Low on Stock", itemDto), itemDto.BusinessId);
        else if (availableStock == 0) SendNotification(CreateNotification("Out of Stock", itemDto), itemDto.BusinessId);
    }

    private void SendNotification(NotificationPatternDto notification, string businessId) {
        var message = new MulticastMessage() {
            Notification = notification.Notification,
            Data = notification.Data,
            Tokens = GetClientDeviceTokens(businessId)
        };
        FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
    }

    private List<string> GetClientDeviceTokens(string businessId) {
        BusinessDto businessDto = _businessRepository.GetBusiness(new(null, businessId)).Result;
        return businessDto!.DeviceKeys;
    }

    private NotificationPatternDto CreateNotification(string title, StoreItemDto itemDto) {
        return new NotificationPatternDto(
            new Notification {
                Title = title,
                Body = $"You are {title.ToLower()} on {itemDto.Name}"
            },
            new Dictionary<string, string> {
                { "SKU", itemDto.SKU },
                { "BusinessId", itemDto.BusinessId },
                { "Category", itemDto.Category },
                { "Name", itemDto.Name },
                { "Stock", itemDto.TotalStock.ToString() },
                { "ImageUrl", itemDto.ImageFilename != null ? _storageService.GetFilePresignedUrl("instock-item-images", itemDto.ImageFilename ?? "").Message : "" }
            }
        );
    }
}