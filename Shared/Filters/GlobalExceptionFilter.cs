using System.Net;
using instock_server_application.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace instock_server_application.Shared.Filters; 

public class GlobalExceptionFilter : IExceptionFilter {

    public void OnException(ExceptionContext context) {

        HttpStatusCode returnStatus = HttpStatusCode.InternalServerError;
        string returnMessage = "A server error occurred.";
        
        if (context.Exception.GetType().IsSubclassOf(typeof(BusinessHttpException))) {
            BusinessHttpException exception = (BusinessHttpException) context.ExceptionDispatchInfo.SourceException;
            
            returnStatus = exception.HttpStatusCode;
            returnMessage = exception.Message;
        }
        
        HttpResponse response = context.HttpContext.Response;
        response.StatusCode = (int) returnStatus;
        response.ContentType = "application/json";
        response.WriteAsync(returnMessage);
        
        context.ExceptionHandled = true;
    }
    
}