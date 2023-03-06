using System.Net;

namespace instock_server_application.Shared.Exceptions; 

// Thrown when the user is trying to create a business but they already own one
public class BusinessNameExceedsMaxLengthException : BusinessHttpException {
    
    public BusinessNameExceedsMaxLengthException(int maxLength) : base(NiceErrorTitle(maxLength), ErrorHttpStatusCode(), NiceErrorMessage(maxLength)) {}

    private static string NiceErrorTitle(int maxLength) {
        return $"The business name exceed the maximum length of {maxLength}";
    }

    private static int ErrorHttpStatusCode() {
        return StatusCodes.Status400BadRequest;
    }

    private static string NiceErrorMessage(int maxLength) {
        return $"The business name cannot exceed {maxLength} characters.";
    }
    
}