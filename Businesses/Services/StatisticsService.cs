using System.Globalization;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Security.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Util.Comparers;
using Newtonsoft.Json;

namespace instock_server_application.Businesses.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IItemRepo _itemRepo;
    private readonly IAccessTokenService _accessTokenService;
    
    public StatisticsService(IItemRepo itemRepo, IAccessTokenService accessTokenService) {
        _itemRepo = itemRepo;
        _accessTokenService = accessTokenService;
    }
    
    public async Task<AllStatsDto?> GetStats(UserDto userDto, string businessId) {
        if (_accessTokenService.CheckBusiness(userDto.UserBusinessId, businessId)) {
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
            
            // Create default stats suggestions 
            var error = new ErrorNotification();
            error.AddError("No Stats Suggestions");
            StatsSuggestionsDto statsSuggestionsDto = new StatsSuggestionsDto(error);

            // User has access, but incorrect businessID or no items found
            if (responseItems.Count == 0) {
                // Return an empty stats object
                return new AllStatsDto(overallShopPerformance, categoryStats, salesByMonth, deductionsByMonth, statsSuggestionsDto);
            }

            // Create list of item dtos to work with
            foreach (Dictionary<string, AttributeValue> item in responseItems) {
                string stock = item["Stock"].S ?? item["Stock"].N;
                StatItemDto statItemDto = new StatItemDto(
                    item["SKU"].S,
                    item["BusinessId"].S,
                    item["Category"].S,
                    item["Name"].S,
                    stock,
                    null
                );
                if (item.ContainsKey("StockUpdates"))
                {
                    string jsonString = item["StockUpdates"].S;
                    List<StatStockDto> statStockDtos = JsonConvert.DeserializeObject<List<StatStockDto>>(jsonString);
                    statItemDto.StockUpdates = statStockDtos;
                }
                statItemDtos.Add(statItemDto);
            }
            
            // Create list of stat item dtos only if stock updates have been made
            // use this version if we only want stats on items with at least one stock change
            
            // foreach (Dictionary<string, AttributeValue> item in responseItems) {
            //     if (item.ContainsKey("StockUpdates"))
            //     {
            //         string jsonString = item["StockUpdates"].S;
            //         List<StatStockDto> statStockDtos = JsonConvert.DeserializeObject<List<StatStockDto>>(jsonString);
            //         string stock = item["Stock"].S ?? item["Stock"].N;
            //         StatItemDto statItemDto = new StatItemDto(
            //             item["SKU"].S,
            //             item["BusinessId"].S,
            //             item["Category"].S,
            //             item["Name"].S,
            //             stock,
            //             statStockDtos
            //         );
            //         statItemDtos.Add(statItemDto);
            //     }
            // }
            
            // get suggestions
            statsSuggestionsDto = GetSuggestions(statItemDtos);
            
            // loop through StatItemDtos and calculate stats
            foreach (var statItemDto in statItemDtos)
            {
                // get category and add blank category stats if not already added
                string category = statItemDto.Category;
                Dictionary<string, int> categoryDict = new Dictionary<string, int>() {
                    {"Sale", 0},
                    {"Order", 0},
                    {"Return", 0},
                    {"Giveaway", 0},
                    {"Damaged", 0},
                    {"Restocked", 0},
                    {"Lost", 0},
                };
                if (!categoryStats.ContainsKey(category))
                {
                    categoryStats.Add(category, categoryDict);
                }
                // loop through each statStockDto
                foreach (var statStockDto in statItemDto.StockUpdates?? Enumerable.Empty<StatStockDto>())
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
                    
                    // Update category stats
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
            return new AllStatsDto(overallShopPerformance, categoryStats, salesByMonth, deductionsByMonth, statsSuggestionsDto);
        }
        // If the user doesn't have access, return "null"
        return null;
    }

        public StatsSuggestionsDto GetSuggestions(List<StatItemDto> statItemDtos)
        {
            SortedDictionary<int, StatItemDto> itemSalesDict = new SortedDictionary<int, StatItemDto>() { {0, null} };
            SortedDictionary<int, StatItemDto> itemReturnsDict = new SortedDictionary<int, StatItemDto>() { {0, null} };
            SortedDictionary<int, StatItemDto> timeNoSalesDict = new SortedDictionary<int, StatItemDto>() { {0, null} };
            Dictionary<string, int> categorySalesDict = new Dictionary<string, int>() { {"No Categories Found", 0} };
            Dictionary<string, StatItemDto> salesStockRatioDict = new();
            // Loop through items
            foreach (var statItemDto in statItemDtos)
            {
                string category = statItemDto.Category;
                string itemStock = statItemDto.Stock;
                int categorySales = 0;
                int itemSales = 0;
                int itemReturns = 0;
                List<DateTime> saleDates = new List<DateTime>();
                DateTime mostRecentSale = DateTime.MinValue;
                // loop through stock updates
                foreach (var statStockDto in statItemDto.StockUpdates?? Enumerable.Empty<StatStockDto>())
                {
                    int amountChanged = Math.Abs(statStockDto.AmountChanged);
                    DateTime dateAdded = DateTime.Parse(statStockDto.DateTimeAdded);
                    // update sales numbers
                    if (statStockDto.ReasonForChange == "Sale")
                    {
                        itemSales += amountChanged;
                        saleDates.Add(dateAdded);
                        categorySales += amountChanged;
                        // calculate most recent sale
                        if (dateAdded > mostRecentSale)
                        {
                            mostRecentSale = dateAdded;
                        }
                    }
                    // calculate return numbers
                    if (statStockDto.ReasonForChange == "Returned")
                    {
                        itemReturns += amountChanged;
                    }
                }
                // if there were sales to compare, 
                if (mostRecentSale != DateTime.MinValue)
                {
                    //then work out item with longest no sales period
                    int daysNoSales = DifferenceInDays(mostRecentSale, DateTime.Now);
                    timeNoSalesDict[daysNoSales] = statItemDto;
                    if (saleDates.Count > 1)
                    {
                        saleDates.Add(DateTime.Now);
                        int timeBetweenSales = AverageDaysBetweenSales(saleDates);
                        // If there were multiple sale dates, calculate time between sales
                        string salesStockRatio = timeBetweenSales + ":" + itemStock;
                        salesStockRatioDict[salesStockRatio] = statItemDto;
                    }
                }
                itemSalesDict[itemSales] = statItemDto;  
                categorySalesDict[category] = categorySales;
                itemReturnsDict[itemReturns] = statItemDto;
            }
            
            // sort category and sales ratio dictionaries to get best and worst performers
            var sortedCategoryDict = categorySalesDict.OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
            var sortedRatioDict = new SortedDictionary<string, StatItemDto>(salesStockRatioDict, new SalesToStockRatioComparer());
            
            // Create suggestions if there is enough data to work with
            Dictionary<int, StatItemDto> bestSellingItem = itemSalesDict.Last().Key > 0 
                ? new Dictionary<int, StatItemDto> { { itemSalesDict.Last().Key, itemSalesDict.Last().Value } }
                : new Dictionary<int, StatItemDto>();

            Dictionary<int, StatItemDto> worstSellingItem = new Dictionary<int, StatItemDto> { { itemSalesDict.First().Key, 
                itemSalesDict.First().Value } };

            Dictionary<int, string> bestSellingCategory = sortedCategoryDict.Last().Value > 0
                ? new Dictionary<int, string> { { sortedCategoryDict.First().Value, sortedCategoryDict.First().Key } }
                : new Dictionary<int, string>();

            Dictionary<int, string> worstSellingCategory = new Dictionary<int, string> { { sortedCategoryDict.Last().Value, 
                sortedCategoryDict.Last().Key } };

            Dictionary<int, StatItemDto> mostReturnedItem = itemReturnsDict.Last().Key > 0 
                ? new Dictionary<int, StatItemDto> { { itemReturnsDict.Last().Key, itemReturnsDict.Last().Value } }
                : new Dictionary<int, StatItemDto>();

            Dictionary<string, StatItemDto> longestNoSales = timeNoSalesDict.Last().Key > 0 
                ? new Dictionary<string, StatItemDto> { { $"{timeNoSalesDict.Last().Key} days", timeNoSalesDict.Last().Value } }
                : new Dictionary<string, StatItemDto>();

            Dictionary<string, StatItemDto> itemToRestock = sortedRatioDict.Count > 0 
                ? new Dictionary<string, StatItemDto> { { sortedRatioDict.Last().Key, sortedRatioDict.Last().Value } }
                : new Dictionary<string, StatItemDto>();

            return new StatsSuggestionsDto(bestSellingItem, worstSellingItem, itemToRestock, longestNoSales, 
                bestSellingCategory, worstSellingCategory, mostReturnedItem);
        }

        public int DifferenceInDays(DateTime date1, DateTime date2)
        {
            TimeSpan difference = date2 - date1;
            int differenceInDays = difference.Days;
            return differenceInDays;
        }

        public int AverageDaysBetweenSales(List<DateTime> saleDates)
        {
            var sortedSaleDates = saleDates.OrderBy(d => d).ToList();
            int totalDays = 0;
            for (int i = 0; i < sortedSaleDates.Count - 1; i++)
            {
                TimeSpan timeDiff = sortedSaleDates[i + 1] - sortedSaleDates[i];
                totalDays += timeDiff.Days;
            }
            int avgDays = totalDays / (sortedSaleDates.Count - 1);
            return avgDays;
        }
}