using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public interface IBusinessConnectionService {
    void SyncAllBusinessesItemsToConnections(object? callingObject);
}