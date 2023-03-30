using instock_server_application.Security.Dtos;

namespace instock_server_application.Security.Services.Interfaces; 

public interface IRefreshTokenService {
    string GenerateRandString();
    string GenerateExpiry();
    void CreateToken(RefreshTokenDto refreshTokenDto);
}