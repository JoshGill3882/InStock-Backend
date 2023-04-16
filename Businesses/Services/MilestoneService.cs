using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Util.Dto;
using instock_server_application.Util.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class MilestoneService : IMilestoneService {
    private readonly IItemRepo _itemRepo;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly INotificationService _notificationService;
    private readonly IUtilService _utilService;

    public MilestoneService(IItemRepo itemRepo, IMilestoneRepository milestoneRepository, INotificationService notificationService, IUtilService utilService) {
        _itemRepo = itemRepo;
        _milestoneRepository = milestoneRepository;
        _notificationService = notificationService;
        _utilService = utilService;
    }

    private async Task<int> GetTotalSales(StoreItemDto itemDto) {
        int totalSales = itemDto.TotalOrders;

        List<StatItemDto> statItemDtos = await _itemRepo.GetAllItemsStatsDetails(itemDto.BusinessId);

        foreach (var statItemDto in statItemDtos) {
            foreach (var statStockDto in statItemDto.StockUpdates ?? Enumerable.Empty<StatStockDto>()) {
                if (statStockDto.ReasonForChange == "Sale") {
                    totalSales += -statStockDto.AmountChanged;
                }
            }
        }

        Console.Write($"{itemDto.Name} has {totalSales} sales\n");
        return totalSales;
    }

    public async Task CheckMilestones(StoreItemDto itemDto) {
        int totalSales = await GetTotalSales(itemDto);
        
        if (totalSales == 10) {
            await CreateMilestone(itemDto, totalSales);
            // _notificationService.MilestoneNotificationChecker(itemDto);
        }
        if (totalSales == 50) {
            await CreateMilestone(itemDto, totalSales);
            // _notificationService.MilestoneNotificationChecker(itemDto);
        }
        if (totalSales == 100) {
            await CreateMilestone(itemDto, totalSales);
            // _notificationService.MilestoneNotificationChecker(itemDto);
        }
        if (totalSales == 200) {
            await CreateMilestone(itemDto, totalSales);
            // _notificationService.MilestoneNotificationChecker(itemDto);
        }
    }

    private async Task CreateMilestone(StoreItemDto itemDto, int totalSales) {
        Console.Write($"You have hit {totalSales} sales on {itemDto.Name}\n");
        
        StoreMilestoneDto milestoneDto = new StoreMilestoneDto(
            Guid.NewGuid().ToString(),
            itemDto.BusinessId,
            itemDto.SKU,
            itemDto.Name,
            itemDto.ImageFilename,
            totalSales,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 
            true
        );

        await _milestoneRepository.SaveNewMilestone(milestoneDto);
    }

    public async Task<ListOfMilestonesDto> GetAllMilestones(UserDto userDto, string businessId) {
        
        if (!_utilService.CheckUserBusinessId(userDto.UserBusinessId, businessId)) {
            ErrorNotification errorNotes = new ErrorNotification();
            errorNotes.AddError(ListOfItemDto.ERROR_UNAUTHORISED);
            return new ListOfMilestonesDto(errorNotes);
        }
        
        List<StoreMilestoneDto> responseItems = await _milestoneRepository.GetAllMilestones(businessId);
        
        return new ListOfMilestonesDto(responseItems);
    }


    // function that gets all sales 🎉
    // function is run on every order and stock update if reason is 'sale' 
    // triggers add milestone to db when milestone is met
        // db stores SKU, Item name, milestone number, timestamp achieved, active (bool)
        // send notification when add milestone is triggered
    // function to deactivate milestone
}