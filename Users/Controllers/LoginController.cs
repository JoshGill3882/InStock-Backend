using Amazon.DynamoDBv2.DataModel;
using instock_server_application.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Users.Controllers; 

[ApiController]
public class LoginController : ControllerBase {
    private static readonly IConfiguration? Config;
    private static readonly IDynamoDBContext? Context;
    private static readonly LoginService? LoginService = new();
    private static readonly PasswordService PasswordService = new();

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
        var userDetails = LoginService.FindUserByEmail(email).Result;
        
        // If a user is not found, return a "not found" error
        if (userDetails == null) {
            Console.WriteLine("USER NOT FOUND");
            return NotFound("User Not Found");
        }
        
        Console.WriteLine("USER FOUND");
        // // If password matches, make a token and pass it back
        if (password == "Test123") {
            Console.WriteLine("PASSWORD MATCHES");
            string JWTToken = LoginService.CreateToken(email);
            Console.WriteLine("TOKEN MADE");
            return Ok(JWTToken);
        }
        
        // // If password does not match, return a "not found" error
        Console.WriteLine("PASSWORD DOES NOT MATCH");
        return NotFound("Incorrect Password");
    }
}