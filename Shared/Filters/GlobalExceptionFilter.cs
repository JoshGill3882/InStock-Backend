using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace instock_server_application.Shared.Filters; 

public class GlobalExceptionFilter : IExceptionFilter {

    private readonly IHostEnvironment _hostEnvironment;

    public GlobalExceptionFilter(IHostEnvironment hostEnvironment) {
        _hostEnvironment = hostEnvironment;
    }

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

        // If we are not running it in dev mode then don't continue with the exception
        // if we are in dev mode then continue throwing the exception to flag it to the developer
        if (!_hostEnvironment.IsDevelopment()) {
            context.ExceptionHandled = true;
        }
    }
    
}