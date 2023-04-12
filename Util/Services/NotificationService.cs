using FirebaseAdmin.Messaging;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
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
                SendNotification(
                    new Notification() {
                        Title = "Low Stock",
                        Body = "You are low on stock on: " + itemDto.Name
                    },
                    new Dictionary<string, string> {
                        { "SKU", itemDto.SKU },
                        { "BusinessId", itemDto.BusinessId },
                        { "Category", itemDto.Category },
                        { "Name", itemDto.Name },
                        { "Stock", itemDto.Stock.ToString() }
                    },
                    itemDto.BusinessId
                );
                break;
            case 0:
                SendNotification(
                    new Notification() {
                        Title = "Out of Stock",
                        Body = "You are out of stock on: " + itemDto.Name
                    },
                    new Dictionary<string, string> {
                        { "SKU", itemDto.SKU },
                        { "BusinessId", itemDto.BusinessId },
                        { "Category", itemDto.Category },
                        { "Name", itemDto.Name },
                        { "Stock", itemDto.Stock.ToString() }
                    },
                    itemDto.BusinessId
                );
                break;
        }
    }

    private void SendNotification(Notification notification, Dictionary<string, string> data, string businessId) {
        var message = new MulticastMessage() {
            Notification = notification,
            Data = data,
            Tokens = GetClientDeviceTokens(businessId)
        };
        FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
    }

    private List<string> GetClientDeviceTokens(string businessId) {
        BusinessDto businessDto = _businessRepository.GetBusiness(new(null, businessId)).Result;
        return businessDto!.DeviceKeys;
    }
}