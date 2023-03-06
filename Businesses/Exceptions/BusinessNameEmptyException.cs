using System.Net;

namespace instock_server_application.Shared.Exceptions; 

// Thrown when the user is trying to create a business but they already own one
public class BusinessNameEmptyException : BusinessHttpException {
    
    public BusinessNameEmptyException() : base(NiceErrorTitle(), ErrorHttpStatusCode(), NiceErrorMessage()) {}

    private static string NiceErrorTitle() {
        return "The business name is empty";
    }

    private static int ErrorHttpStatusCode() {
        return StatusCodes.Status400BadRequest;
    }

    private static string NiceErrorMessage() {
        return "The business name cannot be empty.";
    }
    
}