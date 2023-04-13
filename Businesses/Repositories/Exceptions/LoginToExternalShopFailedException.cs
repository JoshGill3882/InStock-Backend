namespace instock_server_application.Businesses.Repositories.Exceptions; 

public class LoginToExternalShopFailedException : Exception {
    
    public LoginToExternalShopFailedException(string message) : base(ErrorMessage(message))
    {
    }

    private static string ErrorMessage(string message)
    {
        return message;
    }
}
