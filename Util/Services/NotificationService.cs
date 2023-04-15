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
    private readonly IItemRepo _itemRepo;

    public NotificationService(IBusinessRepository businessRepository, IStorageService storageService, IItemRepo itemRepo) {
        _businessRepository = businessRepository;
        _storageService = storageService;
        _itemRepo = itemRepo;
    }

    public NotificationService() {
    }

    public void StockNotificationChecker(StoreItemDto itemDto) {
        int availableStock = itemDto.TotalStock - itemDto.TotalOrders;
        if (availableStock <= 5 && availableStock > 0)
            SendNotification(CreateNotification("Low on Stock", itemDto), itemDto.BusinessId);
        else if (availableStock == 0) SendNotification(CreateNotification("Out of Stock", itemDto), itemDto.BusinessId);
    }

    public async void MilestoneNotificationChecker(StoreItemDto itemDto) {
        List<StatItemDto> statItemDtos = await _itemRepo.GetAllItemsStatsDetails(itemDto.BusinessId);
        int totalSales = itemDto.TotalOrders;
        foreach (var statItemDto in statItemDtos) {
            foreach (var statStockDto in statItemDto.StockUpdates ?? Enumerable.Empty<StatStockDto>()) {
                if (statStockDto.ReasonForChange == "Sale") {
                    totalSales += -statStockDto.AmountChanged;
                }
            }
        }
        Console.Write($"{itemDto.Name} has {totalSales} sales\n");

        switch (totalSales) {
            case 10:
                SendNotification(CreateMilestoneNotification(10, itemDto), itemDto.BusinessId);
                break;
            case 50:
                SendNotification(CreateMilestoneNotification(50, itemDto), itemDto.BusinessId);
                break;
            case 100:
                SendNotification(CreateMilestoneNotification(100, itemDto), itemDto.BusinessId);
                break;
            case 200:
                SendNotification(CreateMilestoneNotification(200, itemDto), itemDto.BusinessId);
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
    
    private NotificationPatternDto CreateMilestoneNotification(int totalSales, StoreItemDto itemDto) {
        return new NotificationPatternDto(
            new Notification {
                Title = "Congrats! You achieved a milestone 🎉",
                Body = $"You have hit {totalSales} sales on {itemDto.Name} 🥳"
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