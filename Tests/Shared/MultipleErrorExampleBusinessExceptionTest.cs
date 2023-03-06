using instock_server_application.Shared.Exceptions;
using Xunit;

namespace instock_server_application.Tests.Shared; 

public class MultipleErrorExampleBusinessExceptionTest {
    
    [Fact]
    public void Constructor_ValidDetails_ReturnsValidException() {

        var expectedTitle = "I'm a teapot.";
        var expectedStatusCode = 418;
        var expectedBodyValue1 = "Affogato is not a type of tea.";
        var expectedBodyValue2 = "The server refuses the attempt to brew coffee with a teapot.";
        
        try {
            throw new MultipleErrorExampleBusinessException();
        }
        catch (BusinessHttpException e) {
            Assert.Equal(expectedTitle, e.HttpResponseTitle);
            Assert.Equal(expectedStatusCode, e.HttpResponseStatusCode);
            Assert.Equal(expectedBodyValue1, e.HttpResponseBody["drinkSelector"]);
            Assert.Equal(expectedBodyValue2, e.HttpResponseBody["error"]);
            Assert.Equal(2, e.HttpResponseBody.Count);
        }
    }
}