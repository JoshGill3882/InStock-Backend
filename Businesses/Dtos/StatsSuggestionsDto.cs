namespace instock_server_application.Businesses.Dtos;

public class StatsSuggestionsDto
{
    public Dictionary<int, StatItemDto> BestSellingItem { get; }
    public Dictionary<int, StatItemDto> WorstSellingItem { get; }
    
    public Dictionary<string, StatItemDto> ItemToRestock { get; }
    public Dictionary<string, StatItemDto> LongestNoSales { get; }
    public Dictionary<int, string> BestSellingCategory { get; }
    public Dictionary<int, string> WorstSellingCategory { get; }
    public Dictionary<int, StatItemDto> MostReturns { get; }

    public StatsSuggestionsDto(Dictionary<int, StatItemDto> bestSellingItem, Dictionary<int, StatItemDto> worstSellingItem, 
        Dictionary<string, StatItemDto> itemToRestock, Dictionary<string, StatItemDto> longestNoSales, 
        Dictionary<int, string> bestSellingCategory, Dictionary<int, string> worstSellingCategory,  
        Dictionary<int, StatItemDto> mostReturns)
    {
        BestSellingItem = bestSellingItem;
        WorstSellingItem = worstSellingItem;
        ItemToRestock = itemToRestock;
        LongestNoSales = longestNoSales;
        BestSellingCategory = bestSellingCategory;
        WorstSellingCategory = worstSellingCategory;
        MostReturns = mostReturns;
    }
}