using System.Security.Claims;
using FluentAssertions;
using instock_server_application.Businesses.Controllers;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Services;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static instock_server_application.Tests.Items.MockData.ItemMock;

namespace instock_server_application.Tests.Items.Controllers; 

public class ItemControllerTest {

    [Fact]
    public async Task Test_GetItem_CorrectItemsForCorrectBusinessId() {
        // Arrange
        const string businessId = "2a36f726-b3a2-11ed-afa1-0242ac120002";
        const string userId = "UID123";
        
        var mockUser = new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>() {
                    new Claim("Id", userId),
                    new Claim("BusinessId", businessId)
                }, "mockUserAuth"));
        List<Dictionary<string, string>> expected = ItemsList();
        
        var mockItemService = new Mock<IItemService>();
        mockItemService.Setup(service => service.GetItems(It.IsAny<UserDto>(), businessId)).Returns(Task.FromResult(expected)!);
        
        var controller = new ItemController(mockItemService.Object);
        controller.ControllerContext = new ControllerContext() {
             HttpContext = new DefaultHttpContext() { User = mockUser }
         };
        
        // Act
        var result = await controller.GetAllItems(businessId);
        
        // Assert
        Assert.IsAssignableFrom<IActionResult>(result);
        var okResult = result as OkObjectResult;
        
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(expected);
    }
    
    [Fact]
    public void Test_GetItem_CorrectItemsForIncorrectBusinessId() {
        // Arrange
        String incorrectBusinessId = "test123";
        const string userId = "UID123";

        List<Dictionary<string, string>> expected = EmptyList();
        var mockUser = new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>() {
                    new Claim("Id", userId),
                    new Claim("BusinessId", incorrectBusinessId)
                }, "mockUserAuth"));
        var mockItemService = new Mock<IItemService>();
        mockItemService.Setup(service => service.GetItems(It.IsAny<UserDto>(), incorrectBusinessId)).Returns(Task.FromResult(expected)!);
        
        var controller = new ItemController(mockItemService.Object);
        controller.ControllerContext = new ControllerContext() {
            HttpContext = new DefaultHttpContext() { User = mockUser }
        };
        
        // Act
        var result = controller.GetAllItems(incorrectBusinessId);
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var notFoundResult = result.Result as NotFoundResult;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public void Test_GetItem_NoAccessForBusinessID() {
        // Arrange
        const string businessId = "2a36f726-b3a2-11ed-afa1-0242ac120002";
        const string userId = "UID123";

        var mockUser = new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>() {
                    new Claim("Id", userId),
                    new Claim("BusinessId", businessId)
                }, "mockUserAuth"));
        
        var mockItemService = new Mock<IItemService>();
        mockItemService.Setup(service => service.GetItems(It.IsAny<UserDto>(), businessId)).Returns(Task.FromResult(ItemsList())!);

        var controller = new ItemController(mockItemService.Object);
        controller.ControllerContext = new ControllerContext() {
            HttpContext = new DefaultHttpContext() { User = mockUser }
        };

        // Act
        var result = controller.GetAllItems(businessId);
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var unauthorizedResult = result.Result as UnauthorizedResult;
        unauthorizedResult?.StatusCode.Should().Be(401);
    }
}