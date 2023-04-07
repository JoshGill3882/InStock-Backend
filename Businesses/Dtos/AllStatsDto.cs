namespace instock_server_application.Businesses.Dtos;

public class AllStatsDto
{
    public Dictionary<string, int> overallShopPerformance { get; }
    
    public Dictionary<string, Dictionary<string, int>> categoryStats { get; }
    
    public Dictionary<int, Dictionary<string, int>> salesByMonth { get; }

    public Dictionary<int, Dictionary<string, int>> deductionsByMonth { get; }

    public AllStatsDto(Dictionary<string, int> overallShopPerformance, Dictionary<string, Dictionary<string, int>> categoryStats, 
        Dictionary<int, Dictionary<string, int>> salesByMonth, Dictionary<int, Dictionary<string, int>> deductionsByMonth)
    {
        this.overallShopPerformance = overallShopPerformance;
        this.categoryStats = categoryStats;
        this.salesByMonth = salesByMonth;
        this.deductionsByMonth = deductionsByMonth;
    }
}