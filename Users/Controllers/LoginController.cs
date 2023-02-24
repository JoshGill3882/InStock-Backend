using instock_server_application.Users.Models;
using instock_server_application.Users.Services;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Users.Controllers; 

[ApiController]
public class LoginController : ControllerBase {
    private readonly ILoginService _loginService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    
    public LoginController(ILoginService loginService, IPasswordService passwordService, IJwtService jwtService) {
        _loginService = loginService;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Method for logging into the system
    /// Passes back a JWT token on success or an error message on failure
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <param name="password"> User's Password </param>
    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password) {
        // Get the User's Details based on the entered email
        User? userDetails = _loginService.FindUserByEmail(email).Result;
        
        // If a user is not found, return a "not found" error
        if (userDetails == null) {
            return NotFound("Invalid Credentials");
        }
        
        // If password matches, make a token and pass it back
        if (_passwordService.Verify(password, userDetails.Password)) {
            string jwtToken = _jwtService.CreateToken(email);
            return Ok(jwtToken);
        }
        
        // If password does not match, return a "not found" error
        return NotFound("Invalid Credentials");
    }
}