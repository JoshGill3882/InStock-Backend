using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IMilestoneRepository {
    Task<StoreMilestoneDto> SaveNewMilestone(StoreMilestoneDto milestoneDto);
}