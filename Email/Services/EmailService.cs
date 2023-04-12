using System.Net.Mail;
using instock_server_application.Email.Objects;
using instock_server_application.Email.Services.Interfaces;

namespace instock_server_application.Email.Services; 

public class EmailService : IEmailService {
    private readonly SmtpClient _smtpClient;

    public EmailService(SmtpClient smtpClient) {
        _smtpClient = smtpClient;
    }

    public EmailResponseObject SendEmailAsync(string subject, string message) {
        EmailResponseObject emailResponse = new EmailResponseObject();
        
        string email = "instockapplication@gmail.com";

        try {
            _smtpClient.SendMailAsync(
                new MailMessage(
                    from: email,
                    to: email,
                    subject,
                    message
                )
            );

            emailResponse.Message = "Your message has been sent! Thank you for contacting us.";
            return emailResponse;
        }
        catch (SmtpException e) {
            emailResponse.StatusCode = (int)e.StatusCode;
            emailResponse.Message =
                "Oops, something went wrong! We were not able to send your message. Please try again later";
            return emailResponse;
        }
    }
}