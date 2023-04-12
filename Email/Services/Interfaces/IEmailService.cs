using instock_server_application.Email.Objects;

namespace instock_server_application.Email.Services.Interfaces; 

public interface IEmailService {
    EmailResponseObject SendEmailAsync(string subject, string message);
}