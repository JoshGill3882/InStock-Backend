using instock_server_application.Users.Models;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Users.Controllers; 

[ApiController]
public class LoginController : ControllerBase {
    private readonly ILoginService _loginService;
    
    public LoginController(ILoginService loginService) {
        _loginService = loginService;
    }

    /// <summary>
    /// Method for logging into the system
    /// Passes back a JWT token on success or an error message on failure
    /// </summary>
    /// <param name="login"> Login model containing users login and password </param>
    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] Login login) {
        Console.WriteLine(login);
        var res = await _loginService.Login(login.Email, login.Password, login.DeviceToken);
        if (res == null) {
            return NotFound("Invalid Credentials");
        }
        return Ok(res);
    }
}