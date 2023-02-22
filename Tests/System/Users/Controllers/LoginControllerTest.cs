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
        const string token = "jwt-t0k3n";
        const string email = "johnbarnes@gmail.com";
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
    
    [Fact]
    public void Test_Login_ReturnsNotFound() {
        // Arrange
        const string token = "jwt-t0k3n";
        const string email = "johnbarnes@gmail.com";
        var mockService = new Mock<ILoginService>();
        mockService.Setup(service => service.CreateToken(email)).Returns(token);
        mockService.Setup(_ => _.FindUserByEmail(email)).Returns(Task.FromResult(SingleUser()));
        var controller = new LoginController(mockService.Object);

        // Act
        var result = controller.Login("","Test123");
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        
        var notFoundObjectResult = result.Result as NotFoundObjectResult;
        notFoundObjectResult.StatusCode.Should().Be(404);
        notFoundObjectResult.Value.Should().Be("User Not Found");
    }
}