using instock_server_application.Security.Services.Interfaces;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services.Interfaces;

namespace instock_server_application.Users.Services; 

public class LoginService : ILoginService {
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IAccessTokenService _accessTokenService;

    public LoginService(IUserService userService, IPasswordService passwordService, IAccessTokenService accessTokenService) {
        _userService = userService;
        _passwordService = passwordService;
        _accessTokenService = accessTokenService;
    }
    
    public async Task<String> Login(string email, string password) {
        string jwtToken = null;
        
        User? userDetails = _userService.FindUserByEmail(email).Result;

        if (userDetails == null) {
            return null;
        }

        // If password matches, make a token and pass it back
        if (_passwordService.Verify(password, userDetails.Password)) {
            jwtToken = _accessTokenService.CreateToken(userDetails.UserId, userDetails.Email, userDetails.BusinessId);
        }
        return jwtToken;
    }
}