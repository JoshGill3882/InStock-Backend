using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Tests.Items.MockData;

public class StatsMock
{
    public static AllStatsDto AllStats()
    {
        return new AllStatsDto(
            new Dictionary<string, int>() {
                {"Sale", 558},
                {"Order", 0},
                {"Return", 0},
                {"Giveaway", 112},
                {"Damaged", 10},
                {"Restocked", 0},
                {"Lost", 0},
                {"Restock", 249},
                {"Just Cuz", 20},
                {"Wholesale", 220},
            },
            new Dictionary<string, Dictionary<string, int>>()
            {
                {
                    "Stickers", new Dictionary<string, int>()
                    {
                        {"Sale", 20},
                        {"Order", 0},
                        {"Return", 0},
                        {"Giveaway", 110},
                        {"Damaged", 0},
                        {"Restocked", 0},
                        {"Lost", 0},
                        {"Restock", 10}
                    }
                },
                {
                    "Water", new Dictionary<string, int>()
                    {
                        {"Sale", 60},
                        {"Order", 0},
                        {"Return", 0},
                        {"Giveaway", 0},
                        {"Damaged", 0},
                        {"Restocked", 0},
                        {"Lost", 0},
                        {"Just Cuz", 20}
                    }
                },
                {
                    "Cards", new Dictionary<string, int>()
                    {
                        {"Sale", 63},
                        {"Order", 0},
                        {"Return", 0},
                        {"Giveaway", 0},
                        {"Damaged", 0},
                        {"Restocked", 0},
                        {"Lost", 0},
                        {"Wholesale", 220},
                        {"Restock", 50}
                    }
                },
                {
                    "Bookmarks", new Dictionary<string, int>()
                    {
                        {"Sale", 375},
                        {"Order", 0},
                        {"Return", 0},
                        {"Giveaway", 2},
                        {"Damaged", 10},
                        {"Restocked", 0},
                        {"Lost", 0},
                        {"Restock", 179}
                    }
                },
                {
                    "category", new Dictionary<string, int>()
                    {
                        {"Sale", 10},
                        {"Order", 0},
                        {"Return", 0},
                        {"Giveaway", 0},
                        {"Damaged", 0},
                        {"Restocked", 0},
                        {"Lost", 0}
                    }
                },
                {
                    "blorp", new Dictionary<string, int>()
                    {
                        {"Sale", 30},
                        {"Order", 0},
                        {"Return", 0},
                        {"Giveaway", 0},
                        {"Damaged", 0},
                        {"Restocked", 0},
                        {"Lost", 0},
                        {"Restock", 10}
                    }
                }
            },
            new Dictionary<int, Dictionary<string, int>>()
            {
                {
                    2023, new Dictionary<string, int>()
                    {
                        {"Apr", 40},
                        {"Mar", 518}
                    }
                }
            },
            new Dictionary<int, Dictionary<string, int>>()
            {
                {
                    2023, new Dictionary<string, int>()
                    {
                        {"Apr", 20},
                        {"Mar", 345}
                    }
                }
            },
            new StatsSuggestionsDto(
                new Dictionary<int, StatItemDto>()
                {
                    {55, new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                        "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                            "2023-03-23T14:52:11.2945412+00:00")})}
                },
                new Dictionary<int, StatItemDto>()
                {
                    {55, new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                        "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                            "2023-03-23T14:52:11.2945412+00:00")})}
                },
                new Dictionary<string, StatItemDto>()
                {
                    {"100: 20", new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                        "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                            "2023-03-23T14:52:11.2945412+00:00")})}
                },
                new Dictionary<string, StatItemDto>()
                {
                    {"100 days", new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                        "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                            "2023-03-23T14:52:11.2945412+00:00")})}
                },
                new Dictionary<int, string>()
                {
                    {200, "Cards"}
                },
                new Dictionary<int, string>()
                {
                    {40, "Bookmarks"}
                },
                new Dictionary<int, StatItemDto>()
                {
                    {55, new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                        "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                            "2023-03-23T14:52:11.2945412+00:00")})}
                }
      
                 // new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                 // "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                 //     "2023-03-23T14:52:11.2945412+00:00")}),
                // new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                //     "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                //         "2023-03-23T14:52:11.2945412+00:00")}),
                // new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                //     "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                //         "2023-03-23T14:52:11.2945412+00:00")}),
                // new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                //     "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                //         "2023-03-23T14:52:11.2945412+00:00")}),
                // new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                //     "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                //         "2023-03-23T14:52:11.2945412+00:00")}),
                // "Bookmarks",
                // "Cards",
                // new StatItemDto("CRD-CKT-RLB", "2a36f726-b3a2-11ed-afa1-0242ac120002", "Cards",
                //     "Birthday Cockatoo", "50", new List<StatStockDto>{new StatStockDto(-100, "Giveaway", 
                //         "2023-03-23T14:52:11.2945412+00:00")})
                )
        );
    }
}