using System.Net;

namespace instock_server_application.Shared.Exceptions; 

[Serializable]
public class BusinessHttpException : Exception {
    
    public string HttpResponseTitle { get; }
    public int HttpResponseStatusCode { get; }
    public Dictionary<string, string> HttpResponseBody { get; }

    public BusinessHttpException() {
    }
    
    public BusinessHttpException(string httpResponseTitle, int httpResponseStatusCode, Dictionary<string, string> httpResponseBody) {
        HttpResponseTitle = httpResponseTitle;
        HttpResponseStatusCode = httpResponseStatusCode;
        HttpResponseBody = httpResponseBody;
    }
    
    public BusinessHttpException(string httpResponseTitle, int httpResponseStatusCode, string httpResponseBody) {
        HttpResponseTitle = httpResponseTitle;
        HttpResponseStatusCode = httpResponseStatusCode;
        HttpResponseBody = new Dictionary<string, string>() {
            {"error", httpResponseBody}
        };
    }
}