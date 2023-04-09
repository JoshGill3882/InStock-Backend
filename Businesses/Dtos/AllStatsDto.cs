namespace instock_server_application.Businesses.Dtos;

public class AllStatsDto
{
    public Dictionary<string, int> OverallShopPerformance { get; }
    
    public Dictionary<string, Dictionary<string, int>> CategoryStats { get; }
    
    public Dictionary<int, Dictionary<string, int>> SalesByMonth { get; }

    public Dictionary<int, Dictionary<string, int>> DeductionsByMonth { get; }
    
    public StatsSuggestionsDto Suggestions { get; }

    public AllStatsDto(Dictionary<string, int> overallShopPerformance, Dictionary<string, Dictionary<string, int>> categoryStats, 
        Dictionary<int, Dictionary<string, int>> salesByMonth, Dictionary<int, Dictionary<string, int>> deductionsByMonth,
        StatsSuggestionsDto suggestions)
    {
        this.OverallShopPerformance = overallShopPerformance;
        this.CategoryStats = categoryStats;
        this.SalesByMonth = salesByMonth;
        this.DeductionsByMonth = deductionsByMonth;
        this.Suggestions = suggestions;
    }
}