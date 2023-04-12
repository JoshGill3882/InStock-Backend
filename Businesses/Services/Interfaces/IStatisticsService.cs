using instock_server_application.Businesses.Dtos;
using instock_server_application.Util.Dto;

namespace instock_server_application.Businesses.Services.Interfaces;

public interface IStatisticsService
{
    public Task<AllStatsDto?> GetStats(UserDto userDto, string businessId);
}