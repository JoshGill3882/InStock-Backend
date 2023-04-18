using System.Text.RegularExpressions;
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
    
    public async Task<string?> Login(string email, string password, string deviceToken) {
        if (!ValidateEmail(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(deviceToken)) { return null; }
        
        User userDetails = _userService.FindUserByEmail(email).Result;

        if (string.IsNullOrEmpty(userDetails.Password) || !_passwordService.Verify(password, userDetails.Password)) return null;
        
        _refreshTokenService.CreateToken(new RefreshTokenDto(userDetails));
            
        string accessToken = _accessTokenService.CreateToken(userDetails.UserId, userDetails.Email, userDetails.BusinessId);

        BusinessDto businessDto = _businessRepository.GetBusiness(new ValidateBusinessIdDto(userDetails.UserId, userDetails.BusinessId)).Result!;

        if (businessDto.DeviceKeys.Contains(deviceToken)) return accessToken;
        
        businessDto.DeviceKeys.Add(deviceToken);
        await _businessRepository.UpdateBusinessDeviceTokens(new BusinessDeviceKeysUpdateModel(userDetails.BusinessId, businessDto.DeviceKeys));

        return accessToken;
    }
    
    /// <summary>
    /// Function for checking if an email is valid based on a Regex
    /// </summary>
    /// <param name="email"> Given email </param>
    /// <returns> true/false based on if the param matches the regex </returns>
    private bool ValidateEmail(string email) {
        return Regex.IsMatch(email, "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\]).{1,320}$");
    }
}