namespace instock_server_application.Security.Services.Interfaces; 

public interface IAccessTokenService {
    public string CreateToken(string id, string email, string businessId);
    public string RefreshToAccess(string refreshToken);
}