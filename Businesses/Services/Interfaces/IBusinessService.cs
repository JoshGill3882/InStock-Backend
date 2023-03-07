using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Services; 

public interface IBusinessService {
    Task<BusinessDto> CreateBusiness(CreateBusinessRequestDto newBusinessRequest);
    public bool CheckBusinessIdInJWT(UserDto userDto, string idToCheck);

}