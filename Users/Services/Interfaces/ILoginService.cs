namespace instock_server_application.Users.Services.Interfaces; 

public interface ILoginService {
    string CreateToken(string email);
    Task<Models.Users> FindUserByEmail(string email);
}