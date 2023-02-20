using Amazon.DynamoDBv2.DataModel;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Users.Controllers; 

[ApiController]
public class LoginController : ControllerBase {
    private static readonly IConfiguration? Config;
    private static readonly IDynamoDBContext? Context;
    private static readonly LoginService? LoginService = new(Config, Context);

    /// <summary>
    /// Method for logging into the system
    /// Passes back a JWT token on success or an error message on failure
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <param name="password"> User's Password </param>
    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public void Login(string email, string password) {
        User userDetails = LoginService.FindUserByEmail(email).Result;

        if (userDetails == null) {
            
        } else {
            // Check user Details match given then send appropriate response
        }
        
        string token = LoginService!.CreateToken(email);
    }

    [HttpGet]
    [Route("/getAllUsers")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers() {
        return Ok(LoginService.GetAllUsers().Result);
    }
}