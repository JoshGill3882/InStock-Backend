using System.Net;
using FluentAssertions;
using instock_server_application.Security.Services.Interfaces;
using instock_server_application.Users.Controllers;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services;
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
        Login login = new Login(email, plainTextPassword);

        
        var mockLoginService = new Mock<ILoginService>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockAccessTokenService = new Mock<IAccessTokenService>();
        var mockUserService = new Mock<IUserService>();
        
        mockLoginService.Setup(service => service.Login(email, plainTextPassword)).Returns(Task.FromResult(token));
        mockUserService.Setup(service => service.FindUserByEmail(email)).Returns(Task.FromResult(SingleUser())!);
        mockPasswordService.Setup(service => service.Verify(plainTextPassword, SingleUser().Password)).Returns(true);
        mockAccessTokenService.Setup(service => service.CreateToken(SingleUser().UserId, SingleUser().Email, SingleUser().BusinessId)).Returns(token);

        var controller = new LoginController(mockLoginService.Object);
        
        // Act
        var result = controller.Login(login);
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        
        var objectResult = result.Result as ObjectResult;
        objectResult.StatusCode.Should().Be(200);
        objectResult.Value.Should().Be(token);
    }
    
    [Fact]
    public void Test_Login_ReturnsNotFoundIfEmailIncorrect() {
        // Arrange
        const string token = "jwt-t0k3n";
        const string email = "incorrectEmail@gmail.com";
        const string plainTextPassword = "Test123";
        Login login = new Login(email, plainTextPassword);
        
        var mockLoginService = new Mock<ILoginService>();
        mockLoginService.Setup(service => service.Login("CorrectEmail@gmail.com", plainTextPassword)).Returns(Task.FromResult(token));
        var controller = new LoginController(mockLoginService.Object);
        
        // Act
        var result = controller.Login(login).Result;
        
        // Assert
        var objectResult = result as ObjectResult;
        Assert.Equal(404, objectResult!.StatusCode);
    }
    
    [Fact]
    public async Task Test_Login_ReturnsNotFoundIfPasswordIncorrect() {
        // Arrange
        const string token = "jwt-t0k3n";
        const string email = "johnbarnes@gmail.com";
        const string plainTextPassword = "IncorrectPassword";
        Login login = new Login(email, plainTextPassword);
        
        var mockLoginService = new Mock<ILoginService>();
        mockLoginService.Setup(service => service.Login(email, "CorrectPassword")).Returns(Task.FromResult(token));
        var controller = new LoginController(mockLoginService.Object);
        
        // Act
        var result = controller.Login(login).Result;
        
        // Assert
        var objectResult = result as ObjectResult;
        Assert.Equal(404, objectResult!.StatusCode);
    }
}