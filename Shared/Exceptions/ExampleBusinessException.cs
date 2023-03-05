using System.Net;

namespace instock_server_application.Shared.Exceptions; 

public class ExampleBusinessException : BusinessHttpException {
    
    public ExampleBusinessException() : base(NiceErrorMessage(), ErrorHttpStatusCode()) {}

    public static string NiceErrorMessage() {
        return "You already own a business.";
    }

    public static HttpStatusCode ErrorHttpStatusCode() {
        return HttpStatusCode.BadRequest;
    }
    
}