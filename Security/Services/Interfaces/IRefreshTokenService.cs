using instock_server_application.Security.Dtos;

namespace instock_server_application.Security.Services.Interfaces; 

public interface IRefreshTokenService {
    public Task<Dictionary<string, string>> CreateToken(RefreshTokenDto refreshTokenDto);
}