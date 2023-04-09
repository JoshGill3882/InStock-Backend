namespace instock_server_application.Businesses.Dtos;

public class StatsSuggestionsDto
{
    public StatItemDto BestSellingItem { get; }
    public StatItemDto WorstSellingItem { get; }
    public StatItemDto HighestSaleStockRatio { get; }
    public StatItemDto LowestSaleStockRatio { get; }
    public StatItemDto LongestNoSales { get; }
    public string BestSellingCategory { get; }
    public string WorstSellingCategory { get; }
    public StatItemDto MostReturns { get; }

    public StatsSuggestionsDto(StatItemDto bestSellingItem, StatItemDto worstSellingItem, StatItemDto highestSaleStockRatio, StatItemDto lowestSaleStockRatio, StatItemDto longestNoSales, string bestSellingCategory, string worstSellingCategory, StatItemDto mostReturns)
    {
        BestSellingItem = bestSellingItem;
        WorstSellingItem = worstSellingItem;
        HighestSaleStockRatio = highestSaleStockRatio;
        LowestSaleStockRatio = lowestSaleStockRatio;
        LongestNoSales = longestNoSales;
        BestSellingCategory = bestSellingCategory;
        WorstSellingCategory = worstSellingCategory;
        MostReturns = mostReturns;
    }
}