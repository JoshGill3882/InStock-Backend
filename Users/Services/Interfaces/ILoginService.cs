namespace instock_server_application.Users.Services.Interfaces; 

public interface ILoginService {
    Task<String?> Login(string email, string password);
}