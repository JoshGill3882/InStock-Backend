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
    /// <param name="email"> User's Email </param>
    /// <param name="password"> User's Password </param>
    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password) {
        var res = _loginService.Login(email, password).Result;
        if (res == null) {
            return NotFound("Invalid Credentials");
        }
        return Ok(res);
    }
}