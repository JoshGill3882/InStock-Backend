namespace instock_server_application.Auth.Services.Interfaces; 

public interface IJwtService {
    public string CreateToken(string id, string email, string businessId);
}