using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Tests.Businesses.MockData
{
    public static class ConnectionMock
    {
        public static ConnectionDto ConnectionDto1 = new ConnectionDto(
            "MockShop", 
            "WeeoWooAuthToken111",
            "TillyScribbles"
        );

        public static ConnectionDto ConnectionDto2 = new ConnectionDto(
            "MockMarket", 
            "WeeoWooAuthToken223",
                "TillyScribbles"
        );
        
        public static List<ConnectionDto> Connections1 = new List<ConnectionDto> {
            ConnectionDto1
        };

        public static List<ConnectionDto> Connections2 = new List<ConnectionDto> {
            ConnectionDto2
        };

        public static StoreConnectionDto GetStoreConnectionDto()
        {
            return new StoreConnectionDto(
                "222", 
                Connections1
            );
        }
        
        public static GetConnectionsRequestDto GetMockConnectionRequest()
        {
            return new GetConnectionsRequestDto("user1", "222", "222");
        }
    }
}