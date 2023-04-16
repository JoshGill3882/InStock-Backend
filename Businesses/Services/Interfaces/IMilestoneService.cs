using instock_server_application.Businesses.Dtos;
using instock_server_application.Util.Dto;

namespace instock_server_application.Businesses.Services.Interfaces; 

public interface IMilestoneService {
    Task CheckMilestones(StoreItemDto itemDto);
    Task<ListOfMilestonesDto> GetAllMilestones(UserDto userDto, string businessId);
}