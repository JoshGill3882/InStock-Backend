using FirebaseAdmin.Messaging;

namespace instock_server_application.Util.Services.Interfaces; 

public interface INotificationService {
    public void SendNotification(Notification notification, Dictionary<string, string> data, string businessId);
}