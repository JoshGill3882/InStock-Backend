using System.Net;

namespace instock_server_application.Shared.Exceptions; 

public class SingleErrorExampleBusinessException : BusinessHttpException {
    
    public SingleErrorExampleBusinessException() : base(NiceErrorTitle(), ErrorHttpStatusCode(), NiceErrorMessage()) {}

    public static string NiceErrorTitle() {
        return "I'm a teapot.";
    }
    
    public static int ErrorHttpStatusCode() {
        return StatusCodes.Status418ImATeapot;
    }
    
    public static string NiceErrorMessage() {
        return "The server refuses the attempt to brew coffee with a teapot.";
    }
    
}