using FluentAssertions;
using instock_server_application.Security.Services.Interfaces;
using instock_server_application.Shared.Services;
using instock_server_application.Shared.Services.Interfaces;
using instock_server_application.Users.Repositories.Interfaces;
using instock_server_application.Tests.Users.MockData;
using instock_server_application.Users.Controllers;
using instock_server_application.Users.Services;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace instock_server_application.Tests.Users.Controllers;

public class CreateAccountTests {

    [Fact]
    public void AccountCreatedWithCorrectDetails() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";
        string testId = "TestID";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockUtilService = new Mock<IUtilService>();
        mockUtilService.Setup(service => service.GenerateUUID()).Returns(testId);
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.EmptyUser())!);
        mockJwtService.Setup(service => service.CreateToken(It.IsAny<string>(), email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");

        
        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UtilService(),
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto("Test", "Test", "test@test.com", "Test123!"));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(201);
        objectResult.Value.Should().Be(expectedJwt);
    }
    
    [Fact]
    public void UnauthorizedWithIncorrectFirstName() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";
        string testId = "TestID";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockUtilService = new Mock<IUtilService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.EmptyUser())!);
        mockJwtService.Setup(service => service.CreateToken(testId, email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        mockUtilService.Setup(service => service.GenerateUUID()).Returns(testId);

        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UtilService(),
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto("", "Test", email, "Test123!"));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(400);
        objectResult.Value.Should().Be("First Name not valid");
    }
    
    [Fact]
    public void UnauthorizedWithIncorrectLastName() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";
        string testId = "TestID";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockUtilService = new Mock<IUtilService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.EmptyUser())!);
        mockJwtService.Setup(service => service.CreateToken(testId, email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        mockUtilService.Setup(service => service.GenerateUUID()).Returns(testId);

        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UtilService(),
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto("Test", "", email, "Test123!"));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(400);
        objectResult.Value.Should().Be("Last Name not valid");
    }
    
    

    [Fact]
    public void UnauthorizedWithIncorrectEmail() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";
        string testId = "TestID";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockUtilService = new Mock<IUtilService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.EmptyUser())!);
        mockJwtService.Setup(service => service.CreateToken(testId, email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        mockUtilService.Setup(service => service.GenerateUUID()).Returns(testId);

        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UtilService(),
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto("Test", "Test", "Incorrect", "Test123!"));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(400);
        objectResult.Value.Should().Be("Email not valid");
    }

    [Fact]
    public void UnauthorizedWithIncorrectPassword() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";
        string testId = "TestID";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockUtilService = new Mock<IUtilService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.EmptyUser())!);
        mockJwtService.Setup(service => service.CreateToken(testId, email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        mockUtilService.Setup(service => service.GenerateUUID()).Returns(testId);

        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UtilService(),
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto("Test", "Test", "test@test.com", "test12"));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(400);
        objectResult.Value.Should().Be("Password not valid");
    }

    [Fact]
    public void UnauthorizedWithDuplicateAccount() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";
        string testId = "TestID";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        var mockUtilService = new Mock<IUtilService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.SampleUser())!);
        mockJwtService.Setup(service => service.CreateToken(testId, email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        mockUtilService.Setup(service => service.GenerateUUID()).Returns(testId);

        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UtilService(),
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto("Test", "Test", email, "Test123!"));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(400);
        objectResult.Value.Should().Be("Duplicate account");
    }
}