using instock_server_application.Businesses.Dtos;
using instock_server_application.Security.Services;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Services; 

public interface IItemConnectionService {

    public Task ConnectItem(UserAuthorisationDto userAuthorisationDto,
        ItemConnectionRequestDto itemConnectionRequestDto);

}