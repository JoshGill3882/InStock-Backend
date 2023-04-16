namespace instock_server_application.Businesses.Repositories.Exceptions; 

public class ExternalConnectionFailedException : Exception {
    
    public ExternalConnectionFailedException(string message) : base(ErrorMessage(message))
    {
    }

    private static string ErrorMessage(string message)
    {
        return message;
    }
}
