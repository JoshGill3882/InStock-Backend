using instock_server_application.Users.Models;

namespace instock_server_application.Users.Services.Interfaces; 

public interface ILoginService {
    Task<User?> FindUserByEmail(string email);
}