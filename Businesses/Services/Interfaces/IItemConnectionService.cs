using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public interface IItemConnectionService {

    public Task<ItemConnectionsDto> ConnectItem(UserAuthorisationDto userAuthorisationDto,
        ItemConnectionRequestDto itemConnectionRequestDto);

}