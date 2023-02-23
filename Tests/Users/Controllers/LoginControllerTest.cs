using FluentAssertions;
using instock_server_application.Users.Controllers;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static instock_server_application.Tests.Users.MockData.UserMock;

namespace instock_server_application.Tests.Users.Controllers; 

public class LoginControllerTest {

    [Fact]
    public void Test_Login_ReturnsJwtToken() {
        // Arrange
        const string token = "jwt-t0k3n";
        const string email = "johnbarnes@gmail.com";
        const string plainTextPassword = "Test123";
        
        var mockLoginService = new Mock<ILoginService>();
        var mockPasswordService = new Mock<IPasswordService>();
        mockLoginService.Setup(service => service.CreateToken(email)).Returns(token);
        mockLoginService.Setup(service => service.FindUserByEmail(email)).Returns(Task.FromResult(SingleUser()));
        mockPasswordService.Setup(service => service.Verify(plainTextPassword, SingleUser().Password)).Returns(true);
        
        var controller = new LoginController(mockLoginService.Object, mockPasswordService.Object);

        // Act
        var result = controller.Login(email, plainTextPassword);
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        
        var okObjectResult = result.Result as OkObjectResult;
        okObjectResult.StatusCode.Should().Be(200);
        okObjectResult.Value.Should().Be(token);
    }
    
    [Fact]
    public void Test_Login_ReturnsNotFoundIfEmailIncorrect() {
        // Arrange
        const string token = "jwt-t0k3n";
        const string email = "johnbarnes@gmail.com";
        const string plainTextPassword = "Test123";
        
        var mockLoginService = new Mock<ILoginService>();
        var mockPasswordService = new Mock<IPasswordService>();
        mockLoginService.Setup(service => service.CreateToken(email)).Returns(token);
        mockLoginService.Setup(service => service.FindUserByEmail(email)).Returns(Task.FromResult(SingleUser()));
        mockPasswordService.Setup(service => service.Verify(plainTextPassword, SingleUser().Password)).Returns(true);
        
        var controller = new LoginController(mockLoginService.Object, mockPasswordService.Object);

        // Act
        var result = controller.Login("test@test.com", plainTextPassword);
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        
        var notFoundObjectResult = result.Result as NotFoundObjectResult;
        notFoundObjectResult.StatusCode.Should().Be(404);
        notFoundObjectResult.Value.Should().Be("Invalid Credentials");
    }
    
    [Fact]
    public void Test_Login_ReturnsNotFoundIfPasswordIncorrect() {
        // Arrange
        const string token = "jwt-t0k3n";
        const string email = "johnbarnes@gmail.com";
        const string plainTextPassword = "Test123";
        
        var mockLoginService = new Mock<ILoginService>();
        var mockPasswordService = new Mock<IPasswordService>();
        mockLoginService.Setup(service => service.CreateToken(email)).Returns(token);
        mockLoginService.Setup(service => service.FindUserByEmail(email)).Returns(Task.FromResult(SingleUser()));
        mockPasswordService.Setup(service => service.Verify(plainTextPassword, SingleUser().Password)).Returns(true);
        
        var controller = new LoginController(mockLoginService.Object, mockPasswordService.Object);

        // Act
        var result = controller.Login(email, "incorrectPassword");
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        
        var notFoundObjectResult = result.Result as NotFoundObjectResult;
        notFoundObjectResult.StatusCode.Should().Be(404);
        notFoundObjectResult.Value.Should().Be("Invalid Credentials");
    }
}