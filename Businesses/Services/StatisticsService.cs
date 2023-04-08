using System.Globalization;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Util.Services.Interfaces;
using Newtonsoft.Json;

namespace instock_server_application.Businesses.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IItemRepo _itemRepo;
    private readonly IUtilService _utilService;
    
    public StatisticsService(IItemRepo itemRepo, IUtilService utilService) {
        _itemRepo = itemRepo;
        _utilService = utilService;
    }
    
        public async Task<AllStatsDto?> GetStats(UserDto userDto, string businessId) {
        
        if (_utilService.CheckUserBusinessId(userDto.UserBusinessId, businessId)) {
            List<Dictionary<string, AttributeValue>> responseItems = _itemRepo.GetAllItems(businessId).Result;
            List<StatItemDto> statItemDtos = new();
            Dictionary<string, Dictionary<string, int>> categoryStats = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<int, Dictionary<string, int>> salesByMonth = new Dictionary<int, Dictionary<string, int>>();
            Dictionary<int, Dictionary<string, int>> deductionsByMonth = new Dictionary<int, Dictionary<string, int>>();
            Dictionary<string, int> overallShopPerformance = new Dictionary<string, int>()
            {
                // Add default values with 0 values
                {"Sale", 0},
                {"Order", 0},
                {"Return", 0},
                {"Giveaway", 0},
                {"Damaged", 0},
                {"Restocked", 0},
                {"Lost", 0},
            };

            // User has access, but incorrect businessID or no items found
            if (responseItems.Count == 0) {
                // Return an empty stats object
                return new AllStatsDto(overallShopPerformance, categoryStats, salesByMonth, deductionsByMonth);
            }

            // Create list of item dtos with stock updates
            foreach (Dictionary<string, AttributeValue> item in responseItems) {
                if (item.ContainsKey("StockUpdates"))
                {
                    string jsonString = item["StockUpdates"].S;
                    List<StatStockDto> statStockDtos = JsonConvert.DeserializeObject<List<StatStockDto>>(jsonString);
                    string stock = item["Stock"].S ?? item["Stock"].N;
                    StatItemDto statItemDto = new StatItemDto(
                        item["SKU"].S,
                        item["BusinessId"].S,
                        item["Category"].S,
                        item["Name"].S,
                        stock,
                        statStockDtos
                    );
                    statItemDtos.Add(statItemDto);
                }
            }
            
            // loop through StatItemDtos and calculate stats
            foreach (var statItemDto in statItemDtos)
            {
                // get category
                string category = statItemDto.Category;
                // loop through each statStockDto
                foreach (var statStockDto in statItemDto.StockUpdates)
                {
                    string reasonForChange = statStockDto.ReasonForChange;
                    int amountChanged = Math.Abs(statStockDto.AmountChanged);
                    int amountChangedWithNegative = statStockDto.AmountChanged;
                    DateTime dateAdded = DateTime.Parse(statStockDto.DateTimeAdded);
                    int yearAdded = dateAdded.Year;
                    string monthAdded = dateAdded.ToString("MMM", CultureInfo.InvariantCulture);
                    
                    // Update overallShopPerformance
                    overallShopPerformance.TryGetValue(reasonForChange, out int reasonAmount); // reasonAmount defaults to 0
                    overallShopPerformance[reasonForChange] = reasonAmount + amountChanged;

                    // Update categoryStats
                    if (!categoryStats.TryGetValue(category, out var categoryDict)) {
                        categoryDict = new Dictionary<string, int>() {
                            {"Sale", 0},
                            {"Order", 0},
                            {"Return", 0},
                            {"Giveaway", 0},
                            {"Damaged", 0},
                            {"Restocked", 0},
                            {"Lost", 0},
                        };
                        categoryStats.Add(category, categoryDict);
                    }

                    categoryDict.TryGetValue(reasonForChange, out int categoryAmount);
                    categoryDict[reasonForChange] = categoryAmount + amountChanged;
                    
                    // Update sales per month
                    if (reasonForChange == "Sale") {
                            if (!salesByMonth.TryGetValue(yearAdded, out var yearDict)) {
                                yearDict = new Dictionary<string, int>();
                                salesByMonth.Add(yearAdded, yearDict);
                            }

                            yearDict.TryGetValue(monthAdded, out int monthAmount);
                            yearDict[monthAdded] = monthAmount + amountChanged;
                    }
                    // Update deductions per month
                    if (reasonForChange != "Sale" && reasonForChange != "Order" && amountChangedWithNegative < 0) {
                        if (!deductionsByMonth.TryGetValue(yearAdded, out var yearDict)) {
                            yearDict = new Dictionary<string, int>();
                            deductionsByMonth.Add(yearAdded, yearDict);
                        }

                        yearDict.TryGetValue(monthAdded, out int monthAmount);
                        yearDict[monthAdded] = monthAmount + amountChanged;
                    }
                } 
            }
            return new AllStatsDto(overallShopPerformance, categoryStats, salesByMonth, deductionsByMonth);
        }

        // If the user doesn't have access, return "null"
        return null;
    }
}