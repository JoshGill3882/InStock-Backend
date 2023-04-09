using System.Security.Claims;
using FluentAssertions;
using instock_server_application.Businesses.Controllers;
using instock_server_application.Businesses.Controllers.forms;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace instock_server_application.Tests.Items.Controllers; 

public class ItemStockControllerTest {
    
    [Fact]
    public async void Test_CreateStockUpdate_() {
        // Variables
        const string userId = "userId123_valid";
        const string userBusinessId = "businessId123_valid";
        const string requestBusinessId = "businessId123_valid";
        const string requestItemSku = "itemSku123_valid";
        const string changeStockAmountBy = "10";
        const string reasonForChange = "Other";
        
        // Arrange
        var mockUser = new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>() {
                    new Claim("Id", userId),
                    new Claim("BusinessId", userBusinessId)
                }, "mockUserAuth"));
        
        var existingItem = new ItemDto(requestItemSku, userBusinessId, "category", "name", 50, "https://image.png");
        var storedItemStockUpdate = new StockUpdateDto(int.Parse(changeStockAmountBy), reasonForChange, DateTime.Today);

        var mockItemRepo = new Mock<IItemRepo>();
        mockItemRepo.Setup(s => s.GetItem(userBusinessId, requestItemSku)).Returns(Task.FromResult(existingItem)!);
        mockItemRepo.Setup(s => s.SaveStockUpdate(It.IsAny<StoreStockUpdateDto>())).Returns(Task.FromResult(storedItemStockUpdate)!);

        var itemStockService = new ItemStockService(mockItemRepo.Object);
        
        var controller = new ItemStockController(itemStockService) {
            ControllerContext = new ControllerContext() {
                HttpContext = new DefaultHttpContext() { User = mockUser }
            }
        };
        
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("urlString");
        controller.Url = mockUrlHelper.Object;
        
        // Act
        CreateStockUpdateForm stockUpdateForm = new CreateStockUpdateForm(changeStockAmountBy, reasonForChange);
        
        IActionResult result = await controller.CreateStockUpdate(requestBusinessId, requestItemSku, stockUpdateForm);
        
        // Assert
        Assert.IsAssignableFrom<IActionResult>(result);
        var unauthorizedResult = result as CreatedResult;
        unauthorizedResult?.StatusCode.Should().Be(201);
        unauthorizedResult?.Value!.GetType().Should().Be<StockUpdateDto>();
    }
}