using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Services.Interfaces; 

public interface IBusinessService {
    Task<BusinessDto> GetBusiness(ValidateBusinessIdDto validateBusinessIdDto);
    Task<BusinessDto> CreateBusiness(CreateBusinessRequestDto newBusinessRequest);
    Task<ConnectionDto> SaveNewConnection(StoreConnectionDto storeConnectionDto);

}