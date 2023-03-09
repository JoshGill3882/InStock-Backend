using instock_server_application.Users.Dtos;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Users.Controllers; 

[ApiController]
[Route("/user")]
public class CreateAccountController : ControllerBase {
    private readonly ICreateAccountService _createAccountService;

    public CreateAccountController(ICreateAccountService createAccountService) {
        _createAccountService = createAccountService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAccount([FromBody] NewAccountDto newAccountDto) {
        string result = _createAccountService.CreateAccount(newAccountDto).Result;

        if (result.Equals("First Name not valid") | result.Equals("Last Name not valid") | result.Equals("Email not valid") | result.Equals("Password not valid") | result.Equals("Duplicate account")) {
            return new ObjectResult(result) { StatusCode = StatusCodes.Status400BadRequest };
        }

        return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
    }
}