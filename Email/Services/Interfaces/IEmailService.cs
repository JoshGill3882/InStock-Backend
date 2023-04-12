using instock_server_application.Email.Dtos;

namespace instock_server_application.Email.Services.Interfaces; 

public interface IEmailService {
    EmailResponseDto SendEmailAsync(string subject, string message);
}