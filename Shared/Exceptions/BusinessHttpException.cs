using System.Net;

namespace instock_server_application.Shared.Exceptions; 

[Serializable]
public class BusinessHttpException : Exception {
    
    public HttpStatusCode HttpStatusCode { get; }

    public BusinessHttpException() {
    }

    public BusinessHttpException(string? message) : base(message) {
    }

    public BusinessHttpException(string? message, HttpStatusCode httpStatusCode) : this(message) {
        HttpStatusCode = httpStatusCode;
    }
}