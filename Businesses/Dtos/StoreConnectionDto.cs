
namespace instock_server_application.Businesses.Dtos
{
    public class StoreConnectionDto
    {
        public string BusinessId { get; }
        public string ShopName { get; }
        public string AuthenticationToken { get; }

        public StoreConnectionDto(string businessId, string shopName, string authenticationToken)
        {
            BusinessId = businessId;
            ShopName = shopName;
            AuthenticationToken = authenticationToken;
        }
    }
}