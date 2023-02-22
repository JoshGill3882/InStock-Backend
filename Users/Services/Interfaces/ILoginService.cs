using instock_server_application.Users.Models;

namespace instock_server_application.Users.Services.Interfaces; 

public interface ILoginService {
    string CreateToken(string email);
    Task<User> FindUserByEmail(string email);
}