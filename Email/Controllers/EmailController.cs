using instock_server_application.Email.Controllers.Forms;
using instock_server_application.Email.Objects;
using instock_server_application.Email.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Email.Controllers; 

[ApiController]
[Route("/contact-us")]
public class EmailController : ControllerBase {
    private readonly IEmailService _emailService;
 
    public EmailController(IEmailService emailService) {
        _emailService = emailService;
    }
 
    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailForm emailForm) {
        EmailResponseObject response = _emailService.SendEmailAsync(
            emailForm.Topic, 
            emailForm.Message
        );

        if (response.StatusCode == 200) {
            return Ok(response.Message);
        }
        
        return BadRequest(response.Message);
    }
}