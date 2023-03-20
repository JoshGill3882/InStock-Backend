using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Services.Interfaces; 

public interface IBusinessService {
    Task<Dictionary<string, string>> GetBusiness(BusinessDto businessDto);
    Task<BusinessDto> CreateBusiness(CreateBusinessRequestDto newBusinessRequest);
}