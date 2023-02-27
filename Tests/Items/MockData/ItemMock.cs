using instock_server_application.Businesses.Models;

namespace instock_server_application.Tests.Items.MockData; 

public static class ItemMock {
    public static List<Item> ItemsList () {
        return new List<Item> {
            new(
                "CRD-CKT-RLB",
                "2a36f726-b3a2-11ed-afa1-0242ac120002",
                "Cards" ,
                "Birthday Cockatoo",
                43
            ),
            new(
                "CRD-BLK-KAL",
                "2a36f726-b3a2-11ed-afa1-0242ac120002",
                "Cards",
                "Blank Koala",
                5
            ),
            new(
                "CRD-BIR-LAA",
                "2a36f726-b3a2-11ed-afa1-0242ac120002",
                "Cards",
                "Birthday LLama",
                35
            ),
            new(
                "BKM-ACD",
                "2a36f726-b3a2-11ed-afa1-0242ac120002",
                "Bookmarks",
                "Magic Avocado",
                57
            )
        };
    }

    public static List<Item> EmptyList() {
        return new();
    }
}