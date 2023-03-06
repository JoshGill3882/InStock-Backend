using System.Net;

namespace instock_server_application.Shared.Exceptions; 

public class MultipleErrorExampleBusinessException : BusinessHttpException {
    
    public MultipleErrorExampleBusinessException() : base(NiceErrorTitle(), ErrorHttpStatusCode(), NiceErrorMessages()) {}

    public static string NiceErrorTitle() {
        return "I'm a teapot.";
    }
    
    public static int ErrorHttpStatusCode() {
        return StatusCodes.Status418ImATeapot;
    }

    public static Dictionary<string, string> NiceErrorMessages() {
        return new Dictionary<string, string>() {
            { "drinkSelector", "Affogato is not a type of tea." },
            { "error", "The server refuses the attempt to brew coffee with a teapot." }
        };
    }
}