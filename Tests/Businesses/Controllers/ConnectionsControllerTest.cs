using System.Security.Claims;
using FluentAssertions;
using instock_server_application.Businesses.Controllers;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Tests.Businesses.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace instock_server_application.Tests.Businesses.Controllers; 

public class ConnectionsControllerTest {
    
    // Will come back to tests
    // [Fact]
    // public async Task Test_GetConnections_CorrectConnectionsForBusinessId() {
    //     // Arrange
    //     StoreConnectionDto expected = ConnectionMock.GetStoreConnectionDto();
    //     GetConnectionsRequestDto getConnectionsRequestDto = ConnectionMock.GetMockConnectionRequest();
    //     
    //      string businessId = getConnectionsRequestDto.BusinessId;
    //      string userId = getConnectionsRequestDto.UserId;
    //     
    //     var mockUser = new ClaimsPrincipal(
    //         new ClaimsIdentity(
    //             new List<Claim>() {
    //                 new Claim("Id", userId),
    //                 new Claim("BusinessId", businessId)
    //             }, "mockUserAuth"));
    //     
    //     var mockConnectionsService = new Mock<IConnectionsService>();
    //     mockConnectionsService.Setup(service => service.GetConnections(getConnectionsRequestDto)).Returns(Task.FromResult(expected)!);
    //     
    //     var controller = new ConnectionsController(mockConnectionsService.Object);
    //     controller.ControllerContext = new ControllerContext() {
    //         HttpContext = new DefaultHttpContext() { User = mockUser }
    //     };
    //     
    //     // Act
    //     var result = await controller.GetAllConnections(businessId);
    //     
    //     // Assert
    //     Assert.IsAssignableFrom<IActionResult>(result);
    //     var okResult = result as OkObjectResult;
    //     
    //     okResult.StatusCode.Should().Be(200);
    //     okResult.Value.Should().Be(expected);
    // }
}