using instock_server_application.Users.Models;
using instock_server_application.Users.Services.Interfaces;

namespace instock_server_application.Users.Services; 

public class LoginService : ILoginService {
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public LoginService(IUserService userService, IPasswordService passwordService, IJwtService jwtService) {
        _userService = userService;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }
    
    public async Task<String> Login(string email, string password) {
        string jwtToken = null;
        
        User? userDetails = _userService.FindUserByEmail(email).Result;

        if (userDetails == null) {
            return null;
        }

        // If password matches, make a token and pass it back
        if (_passwordService.Verify(password, userDetails.Password)) {
            jwtToken = _jwtService.CreateToken(userDetails);
        }
        return jwtToken;
    }
}