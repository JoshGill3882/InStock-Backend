using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IMilestoneRepository {
    Task<MilestoneDto> SaveNewMilestone(MilestoneDto milestoneDto);
    Task<List<MilestoneDto>> GetAllMilestones(string businessId);
    Task<MilestoneDto> HideMilestone(HideMilestoneDto hideMilestoneDto);
}