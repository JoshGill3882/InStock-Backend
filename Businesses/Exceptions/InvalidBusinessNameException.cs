using System.Net;

namespace instock_server_application.Shared.Exceptions; 

public class InvalidBusinessNameException : BusinessHttpException {
    
    public InvalidBusinessNameException(Dictionary<string, string> errors) : base(NiceErrorTitle(), ErrorHttpStatusCode(), errors) {}

    public static string NiceErrorTitle() {
        return "Invalid Business Name";
    }
    
    public static int ErrorHttpStatusCode() {
        return StatusCodes.Status400BadRequest;
    }
}