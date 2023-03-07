using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Services.Interfaces; 

public interface IBusinessService {
    Task<BusinessDto> CreateBusiness(CreateBusinessRequestDto newBusinessRequest);
    public bool CheckBusinessIdInJwt(UserDto userDto, string idToCheck);

}