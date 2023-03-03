using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Services; 

public interface IBusinessService {
    Task<bool> CreateBusiness(UserDto userDto, CreateBusinessDto newBusiness);
    public bool CheckBusinessIdInJWT(UserDto userDto, string idToCheck);

}