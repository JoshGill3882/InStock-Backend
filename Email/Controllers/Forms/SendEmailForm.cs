namespace instock_server_application.Email.Controllers.Forms; 

public class SendEmailForm {
    public string Topic { get; }
    public string Message { get; }

    public SendEmailForm(string topic, string message) {
        Topic = topic;
        Message = message;
    }
}