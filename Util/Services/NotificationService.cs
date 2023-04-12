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

    public void SendNotification(Notification notification, Dictionary<string, string> data, string businessId) {
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