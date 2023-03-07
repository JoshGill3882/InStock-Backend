using System.Net;
using System.Net.Http.Headers;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace instock_server_application.Shared.Filters; 

public class GlobalExceptionFilter : IExceptionFilter {

    public void OnException(ExceptionContext context) {
        
        string responseTitle = "A server error occurred.";
        int responseStatus = StatusCodes.Status500InternalServerError;
        Dictionary<string, string> responseBody = new Dictionary<string, string>() {
            { "otherErrors", "There has been and internal server error." }
        };

        HttpResponse exceptionResponse = context.HttpContext.Response;
        exceptionResponse.StatusCode = responseStatus;
        exceptionResponse.ContentType = "application/json";
        var responseBodyJson = JsonConvert.SerializeObject(new {
            title = responseTitle,
            status = responseStatus,
            errors = responseBody
        });
        exceptionResponse.ContentLength = responseBodyJson.Length;
        exceptionResponse.WriteAsync(responseBodyJson);

        context.ExceptionHandled = true;
    }
    
}