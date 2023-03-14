using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Tests.Items.MockData; 

public static class ItemMock {
    public static List<Dictionary<string, string>> ItemsList () {
        return new List<Dictionary<string, string>> {
            new() {
                { "SKU", "CRD-CKT-RLB" },
                { "BusinessId", "2a36f726-b3a2-11ed-afa1-0242ac120002" },
                { "Category", "Cards" } ,
                { "Name", "Birthday Cockatoo" },
                { "Stock", "43" }
            },
            new() {
                { "SKU", "CRD-CKT-RLB" },
                { "BusinessId", "2a36f726-b3a2-11ed-afa1-0242ac120002" },
                { "Category", "Cards" } ,
                { "Name", "Birthday Cockatoo" },
                { "Stock", "43" }
            },
            new() {
                { "SKU", "CRD-CKT-RLB" },
                { "BusinessId", "2a36f726-b3a2-11ed-afa1-0242ac120002" },
                { "Category", "Cards" } ,
                { "Name", "Birthday Cockatoo" },
                { "Stock", "43" }
            },
            new() {
                { "SKU", "CRD-CKT-RLB" },
                { "BusinessId", "2a36f726-b3a2-11ed-afa1-0242ac120002" },
                { "Category", "Cards" } ,
                { "Name", "Birthday Cockatoo" },
                { "Stock", "43" }
            }
        };
    }

    public static List<Dictionary<string, string>> EmptyList() {
        return new();
    }
}