using FluentAssertions;
using instock_server_application.Businesses.Controllers;
using instock_server_application.Businesses.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static instock_server_application.Tests.Items.MockData.ItemMock;

namespace instock_server_application.Tests.Items.Controllers; 

public class ItemControllerTest {

    [Fact]
    public void Test_GetItem_CorrectItemsForCorrectBusinessId() {
        // Arrange
        const string businessId = "2a36f726-b3a2-11ed-afa1-0242ac120002";
        List<Dictionary<string, string>> expected = ItemsList();
        var mockItemService = new Mock<IItemService>();
        var mockBusinessService = new Mock<IBusinessService>();
        mockItemService.Setup(service => service.GetItems(businessId)).Returns(Task.FromResult(expected));
        var controller = new ItemController(mockItemService.Object, mockBusinessService.Object);

        // Act
        var result = controller.GetAllItems(businessId);
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var okResult = result.Result as OkObjectResult;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(expected);
    }
    
    [Fact]
    public void Test_GetItem_CorrectItemsForIncorrectBusinessId() {
        // Arrange
        const string businessId = "2a36f726-b3a2-11ed-afa1-0242ac120002";
        var mockItemService = new Mock<IItemService>();
        var mockBusinessService = new Mock<IBusinessService>();
        mockItemService.Setup(service => service.GetItems(businessId)).Returns(Task.FromResult(ItemsList()));
        var controller = new ItemController(mockItemService.Object, mockBusinessService.Object);

        // Act
        var result = controller.GetAllItems("test123");
        
        // Assert
        Assert.IsAssignableFrom<Task<IActionResult>>(result);
        var okResult = result.Result as NotFoundObjectResult;
        okResult.StatusCode.Should().Be(404);
        okResult.Value.Should().Be("No Items Found");
    }
}