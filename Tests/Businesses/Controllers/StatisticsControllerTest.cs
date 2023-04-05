using System.Security.Claims;
using FluentAssertions;
using instock_server_application.Businesses.Controllers;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static instock_server_application.Tests.Items.MockData.StatsMock;


namespace instock_server_application.Tests.Businesses.Controllers;

public class StatisticsControllerTest
{
    [Fact]
    public async Task Test_GetStats_CorrectStatsForCorrectBusinessId() {
        // Arrange
        const string businessId = "c4866530-ca3b-4145-a494-7c21f610cd40";
        const string userId = "UID123";
        
        var mockUser = new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>() {
                    new Claim("Id", userId),
                    new Claim("BusinessId", businessId)
                }, "mockUserAuth"));
        AllStatsDto expected = AllStats();
        
        var mockStatisticsService = new Mock<IStatisticsService>();
        mockStatisticsService.Setup(service => service.GetStats(It.IsAny<UserDto>(), businessId)).Returns(Task.FromResult(expected)!);
        
        var controller = new StatisticsController(mockStatisticsService.Object);
        controller.ControllerContext = new ControllerContext() {
            HttpContext = new DefaultHttpContext() { User = mockUser }
        };
        
        // Act
        var result = await controller.GetStatistics(businessId);
        
        // Assert
        Assert.IsAssignableFrom<IActionResult>(result);
        var okResult = result as OkObjectResult;
        
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(expected);
    }
}