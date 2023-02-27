using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Services; 

public interface IBusinessService {
    bool CreateBusiness(UserDto userDto, CreateBusinessDto newBusiness);
}