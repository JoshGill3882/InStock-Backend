using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Security.Dtos;
using instock_server_application.Security.Services.Interfaces;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services.Interfaces;

namespace instock_server_application.Users.Services; 

public class LoginService : ILoginService {
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IBusinessRepository _businessRepository;

    public LoginService(IUserService userService, IPasswordService passwordService, IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService, IBusinessRepository businessRepository) {
        _userService = userService;
        _passwordService = passwordService;
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
        _businessRepository = businessRepository;
    }
    
    public async Task<String?> Login(string email, string password, string deviceToken) {
        User? userDetails = _userService.FindUserByEmail(email).Result;

        if (userDetails == null) { return null; }

        // If password matches, make a token and pass it back
        if (_passwordService.Verify(password, userDetails.Password)) {
            _refreshTokenService.CreateToken(new RefreshTokenDto(userDetails));
            
            string accessToken = _accessTokenService.CreateToken(userDetails.UserId, userDetails.Email, userDetails.BusinessId);

            BusinessDto businessDto = _businessRepository.GetBusiness(new ValidateBusinessIdDto(userDetails.UserId, userDetails.BusinessId)).Result!;
            if (!businessDto.DeviceKeys.Contains(deviceToken)) {
                businessDto.DeviceKeys.Add(deviceToken);
                _businessRepository.UpdateBusinessDeviceTokens(new BusinessDeviceKeysUpdateModel(userDetails.BusinessId, businessDto.DeviceKeys));
            }
            
            return accessToken;
        }
        return null;
    }
}