using System.Net;

namespace instock_server_application.Shared.Exceptions; 

// Thrown when the user is trying to create a business but they already own one
public class UserAlreadyOwnsBusinessException : BusinessHttpException {
    
    public UserAlreadyOwnsBusinessException() : base(NiceErrorTitle(), ErrorHttpStatusCode(), NiceErrorMessage()) {}

    private static string NiceErrorTitle() {
        return "A Business is already associated with your account.";
    }

    private static int ErrorHttpStatusCode() {
        return StatusCodes.Status400BadRequest;
    }

    private static string NiceErrorMessage() {
        return "Your account already owns a business.";
    }
    
}