namespace instock_server_application.Security.Services.Interfaces; 

public interface IJwtService {
    public string CreateToken(string id, string email, string businessId);
}