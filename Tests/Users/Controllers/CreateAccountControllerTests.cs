using Amazon.DynamoDBv2;
using FluentAssertions;
using instock_server_application.Users.Repositories.Interfaces;
using instock_server_application.Tests.Users.MockData;
using instock_server_application.Users.Controllers;
using instock_server_application.Users.Dtos;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace instock_server_application.Tests.Users.Controllers;

public class CreateAccountTests {

    [Fact]
    public void AccountCreatedWithCorrectDetails() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockAmazonDynamoDb = new Mock<IAmazonDynamoDB>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.EmptyUser())!);
        mockJwtService.Setup(service => service.CreateToken(email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        
        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UserService(mockAmazonDynamoDb.Object), // Will need to be switched once Repo Refactor is complete and UserService passes to Repo Layer
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto(email));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(200);
        objectResult.Value.Should().Be(expectedJwt);
    }

    [Fact]
    public void UnauthorizedWithIncorrectEmail() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockAmazonDynamoDb = new Mock<IAmazonDynamoDB>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.EmptyUser())!);
        mockJwtService.Setup(service => service.CreateToken(email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        
        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UserService(mockAmazonDynamoDb.Object), // Will need to be switched once Repo Refactor is complete and UserService passes to Repo Layer
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto("incorrectEmail"));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(401);
        objectResult.Value.Should().Be("Email not valid");
    }

    [Fact]
    public void UnauthorizedWithIncorrectPassword() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockAmazonDynamoDb = new Mock<IAmazonDynamoDB>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.EmptyUser())!);
        mockJwtService.Setup(service => service.CreateToken(email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        
        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UserService(mockAmazonDynamoDb.Object), // Will need to be switched once Repo Refactor is complete and UserService passes to Repo Layer
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(
            new NewAccountDto(
                "Test",
                "Test",
                "test@test.com",
                "test12"
            )
        );

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(401);
        objectResult.Value.Should().Be("Password not valid");
    }

    [Fact]
    public void UnauthorizedWithDuplicateAccount() {
        // Arrange
        string email = "test@test.com";
        string expectedJwt = "TestJWT";

        var mockUserRepo = new Mock<IUserRepo>();
        var mockAmazonDynamoDb = new Mock<IAmazonDynamoDB>();
        var mockJwtService = new Mock<IJwtService>();
        var mockPasswordService = new Mock<IPasswordService>();
        mockUserRepo.Setup(repo => repo.GetByEmail(email)).Returns(Task.FromResult(CreateAccountMockData.SampleUser())!);
        mockJwtService.Setup(service => service.CreateToken(email, "")).Returns(expectedJwt);
        mockPasswordService.Setup(service => service.Encrypt("Test123!")).Returns("s0m3encrypt3dpa55w0rd");
        
        ICreateAccountService createAccountService = new CreateAccountService(
            mockUserRepo.Object,
            new UserService(mockAmazonDynamoDb.Object), // Will need to be switched once Repo Refactor is complete and UserService passes to Repo Layer
            new PasswordService(),
            mockJwtService.Object
        );
        CreateAccountController createAccountController = new CreateAccountController(createAccountService);
        
        // Act
        var result = createAccountController.CreateAccount(CreateAccountMockData.SampleDto(email));

        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var objectResult = result.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(401);
        objectResult.Value.Should().Be("Duplicate account");
    }
}