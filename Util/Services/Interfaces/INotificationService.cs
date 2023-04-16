using FirebaseAdmin.Messaging;
using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Util.Services.Interfaces; 

public interface INotificationService {
    public void StockNotificationChecker(StoreItemDto itemDto);
    public void TriggerMilestoneNotification(StoreItemDto itemDto, int totalSales);
}