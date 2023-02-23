using instock_server_application.Users.Models;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Users.Controllers; 

[ApiController]
public class LoginController : ControllerBase {
    private readonly ILoginService _loginService;
    private readonly IPasswordService _passwordService;
    
    public LoginController(ILoginService loginService, IPasswordService passwordService) {
        _loginService = loginService;
        _passwordService = passwordService;
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
            Console.WriteLine("INVALID CREDENTIALS");
            return NotFound("Invalid Credentials");
        }
        
        Console.WriteLine("USER FOUND");
        // If password matches, make a token and pass it back
        if (_passwordService.Verify(password, userDetails.Password)) {
            Console.WriteLine("PASSWORD MATCHES");
            string jwtToken = _loginService.CreateToken(email);
            Console.WriteLine("TOKEN MADE");
            return Ok(jwtToken);
        }
        
        // If password does not match, return a "not found" error
        Console.WriteLine("INVALID CREDENTIALS");
        return NotFound("Invalid Credentials");
    }
}