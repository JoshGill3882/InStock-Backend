using instock_server_application.Shared.Exceptions;
using Xunit;

namespace instock_server_application.Tests.Shared; 

public class SingleErrorExampleBusinessExceptionTest {
    
    [Fact]
    public void Constructor_ValidDetails_ReturnsValidException() {

        var expectedTitle = "I'm a teapot.";
        var expectedStatusCode = 418;
        var expectedBodyValue = "The server refuses the attempt to brew coffee with a teapot.";
        
        try {
            throw new SingleErrorExampleBusinessException();
        }
        catch (BusinessHttpException e) {
            Assert.Equal(expectedTitle, e.HttpResponseTitle);
            Assert.Equal(expectedStatusCode, e.HttpResponseStatusCode);
            Assert.Equal(expectedBodyValue, e.HttpResponseBody["error"]);
            Assert.Single(e.HttpResponseBody);
        }
    }
}