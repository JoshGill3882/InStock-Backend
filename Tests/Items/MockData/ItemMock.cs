namespace instock_server_application.Tests.Items.MockData; 

public static class ItemMock {
    public static List<Dictionary<string, string>> ItemsList () {
        return new List<Dictionary<string, string>> {
            new() {
                { "SKU", "CRD-CKT-RLB" },
                { "BusinessId", "2a36f726-b3a2-11ed-afa1-0242ac120002" },
                { "Category", "Cards" },
                { "Name", "Birthday Cockatoo" },
                { "Stock", "43" }
            },
            new() {
                { "SKU", "CRD-BLK-KAL" },
                { "BusinessId", "2a36f726-b3a2-11ed-afa1-0242ac120002" },
                { "Category", "Cards" },
                { "Name", "Blank Koala" },
                { "Stock", "5" }
            },
            new() {
                { "SKU", "CRD-BIR-LAA" },
                { "BusinessId", "2a36f726-b3a2-11ed-afa1-0242ac120002" },
                { "Category", "Cards" },
                { "Name", "Birthday LLama" },
                { "Stock", "35" }
            },
            new() {
                { "SKU", "BKM-ACD" },
                { "BusinessId", "2a36f726-b3a2-11ed-afa1-0242ac120002" },
                { "Category", "Bookmarks" },
                { "Name", "Magic Avocado" },
                { "Stock", "57" }
            }
        };
    }
}