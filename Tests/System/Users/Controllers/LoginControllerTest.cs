using FluentAssertions;
using instock_server_application.Users.Controllers;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static instock_server_application.tests.System.Users.MockData.UserMock;

namespace instock_server_application.tests.System.Users.Controllers; 

public class LoginControllerTest {

    [Fact]
    public void Test_Login_ReturnsJwtToken() {
        // Arrange
        var token = "jwt-t0k3n";
        var email = "johnbarnes@gmail.com";
        var mockService = new Mock<ILoginService>();
        mockService.Setup(service => service.CreateToken(email)).Returns(token);
        mockService.Setup(_ => _.FindUserByEmail(email)).Returns(Task.FromResult(SingleUser()));
        var controller = new LoginController(mockService.Object);

        // Act
        var result = controller.Login(email, "Test123");
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        
        var okObjectResult = result.Result as OkObjectResult;
        okObjectResult.StatusCode.Should().Be(200);
        okObjectResult.Value.Should().Be(token);
    }
}