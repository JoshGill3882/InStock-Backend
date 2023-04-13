using FirebaseAdmin.Messaging;

namespace instock_server_application.Util.Dto; 

public class NotificationPatternDto {
    public Notification Notification { get; }
    public Dictionary<string, string> Data { get; }

    public NotificationPatternDto(Notification notification, Dictionary<string, string> data) {
        Notification = notification;
        Data = data;
    }
}