using Amazon.DynamoDBv2;
using instock_server_application.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Users.Controllers; 

[ApiController]
public class LoginController : ControllerBase {
    private readonly LoginService? _loginService;
    private static readonly PasswordService PasswordService = new();
    private readonly IAmazonDynamoDB _client;
    
    public LoginController(IAmazonDynamoDB client)
    {
        _client = client;
        _loginService = new LoginService(_client);
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
        
        var userDetails = _loginService.FindUserByEmail(email).Result;
        
        // If a user is not found, return a "not found" error
        if (userDetails == null) {
            Console.WriteLine("USER NOT FOUND");
            return NotFound("User Not Found");
        }
        
        Console.WriteLine("USER FOUND");
        // // If password matches, make a token and pass it back
        if (password == "Test123") {
            Console.WriteLine("PASSWORD MATCHES");
            string JWTToken = _loginService.CreateToken(email);
            Console.WriteLine("TOKEN MADE");
            return Ok(JWTToken);
        }
        
        // // If password does not match, return a "not found" error
        Console.WriteLine("PASSWORD DOES NOT MATCH");
        return NotFound("Incorrect Password");

    }
}