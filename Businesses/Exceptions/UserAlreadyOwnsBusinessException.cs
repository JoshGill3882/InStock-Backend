using System.Net;

namespace instock_server_application.Shared.Exceptions; 

public class UserAlreadyOwnsBusinessException : BusinessHttpException {
    
    public UserAlreadyOwnsBusinessException() : base(NiceErrorMessage(), ErrorHttpStatusCode()) {}

    public static string NiceErrorMessage() {
        return "You already have a business associated with your account.";
    }

    public static HttpStatusCode ErrorHttpStatusCode() {
        return HttpStatusCode.BadRequest;
    }
    
}