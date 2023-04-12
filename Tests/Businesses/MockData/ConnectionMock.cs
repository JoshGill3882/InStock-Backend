using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Tests.Businesses.MockData
{
    public static class ConnectionMock
    {
        public static ConnectionDto connectionDto1 = new ConnectionDto(
            "MockShop", 
            "WeeoWooAuthToken111"
        );

        public static ConnectionDto connectionDto2 = new ConnectionDto(
            "MockShop2", 
            "WeeoWooAuthToken223"
        );
        
        public static List<ConnectionDto> connections1 = new List<ConnectionDto> {
            connectionDto1
        };

        public static List<ConnectionDto> connections2 = new List<ConnectionDto> {
            connectionDto2
        };

        public static StoreConnectionDto GetStoreConnectionDto()
        {
            return new StoreConnectionDto(
                "222", 
                connections1
            );
        }
        
        public static GetConnectionsRequestDto GetMockConnectionRequest()
        {
            return new GetConnectionsRequestDto("user1", "222", "222");
        }
    }
}