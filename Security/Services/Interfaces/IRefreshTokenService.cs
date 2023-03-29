using instock_server_application.Security.Dtos;

namespace instock_server_application.Security.Services.Interfaces; 

public interface IRefreshTokenService {
    public string GenerateRandString();
    public string GenerateExpiry();
    public Task<string> CreateToken(RefreshTokenDto refreshTokenDto);
}