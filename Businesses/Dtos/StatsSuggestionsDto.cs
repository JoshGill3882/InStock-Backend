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
}