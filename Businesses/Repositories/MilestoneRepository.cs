using Amazon.DynamoDBv2.DataModel;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;
using Newtonsoft.Json;

namespace instock_server_application.Businesses.Repositories; 

public class MilestoneRepository : IMilestoneRepository {
    private readonly IDynamoDBContext _context;

    public MilestoneRepository(IDynamoDBContext context) {
        _context = context;
    }

    public async Task<StoreMilestoneDto> SaveNewMilestone(StoreMilestoneDto milestoneDto) {
        MilestoneModel milestone = new MilestoneModel(
            milestoneDto.MilestoneId,
            milestoneDto.BusinessId,
            milestoneDto.ItemSku,
            milestoneDto.ItemName,
            milestoneDto.TotalSales,
            milestoneDto.DateTime,
            milestoneDto.DisplayMilestone
        );
        
        Console.Write(JsonConvert.SerializeObject(milestoneDto));
        
        await _context.SaveAsync(milestone);

        return milestoneDto;
    }
    
    // deactivate milestone method
    
    // get current milestones
    // public async Task<List<StoreMilestoneDto>> GetMilestones(string businessId) {
    //     
    // }
}