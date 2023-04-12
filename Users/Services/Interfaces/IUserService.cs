using instock_server_application.Users.Dtos;
using instock_server_application.Users.Models;

namespace instock_server_application.Users.Services.Interfaces; 

public interface IUserService {
    Task<User?> FindUserByEmail(string email);
    Task<AccountDetailsDto> GetUser(string email);
}