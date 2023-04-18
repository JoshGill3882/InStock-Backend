using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Services.Interfaces
{
    public interface IConnectionsService
    {
        Task<StoreConnectionDto> CreateConnections(CreateConnectionRequestDto createConnectionRequestDto);
        Task<StoreConnectionDto?> GetConnections(GetConnectionsRequestDto getConnectionsRequestDto);
        Task<ExternalShopAuthenticationTokenDto> ConnectToExternalShop(CreateConnectionForm connectionRequestDetails);

        Task<bool> ValidateBusinessConnectedToPlatform(UserAuthorisationDto userAuthorisationDto, string businessId,
            string platformName);

    }
}