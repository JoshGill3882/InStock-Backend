using FirebaseAdmin.Messaging;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Util.Dto;
using instock_server_application.Util.Services.Interfaces;

namespace instock_server_application.Util.Services; 

public class NotificationService : INotificationService {
    private readonly IBusinessRepository _businessRepository;

    public NotificationService(IBusinessRepository businessRepository) {
        _businessRepository = businessRepository;
    }

    public void StockNotificationChecker(StoreItemDto itemDto) {
        switch (itemDto.Stock) {
            case 5:
                SendNotification(CreateNotification("Low on Stock", itemDto), itemDto.BusinessId);
                break;
            case 0:
                SendNotification(CreateNotification("Out of Stock", itemDto), itemDto.BusinessId);
                break;
        }
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
                Body = $"You are {title.ToLower()} on: {itemDto.Name}"
            },
            new Dictionary<string, string> {
                { "SKU", itemDto.SKU },
                { "BusinessId", itemDto.BusinessId },
                { "Category", itemDto.Category },
                { "Name", itemDto.Name },
                { "Stock", itemDto.Stock.ToString() }
            }
        );
    }
}