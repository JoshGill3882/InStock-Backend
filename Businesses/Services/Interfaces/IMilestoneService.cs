using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Services.Interfaces; 

public interface IMilestoneService {
    Task CheckMilestones(StoreItemDto itemDto);
}