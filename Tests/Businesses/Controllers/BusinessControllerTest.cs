// using System.Security.Claims;
// using instock_server_application.Businesses.Controllers;
// using instock_server_application.Businesses.Controllers.forms;
// using instock_server_application.Businesses.Dtos;
// using instock_server_application.Businesses.Services;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using Xunit;
//
// namespace instock_server_application.Tests.Businesses.Controllers; 
//
// public class BusinessControllerTest {
//
//     // https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
//     [Fact]
//     // public async Task CreateBusiness_ValidDetails_ReturnsOk() {
//     //     string userId = "UID123";
//     //     string businessId = "BID123";
//     //     string BusinessName = "TestBusiness";
//     //
//     //     CreateBusinessForm newBusinessForm = new CreateBusinessForm(BusinessName);
//     //     var mockUser = new ClaimsPrincipal(
//     //         new ClaimsIdentity(
//     //             new List<Claim>() {
//     //                 new Claim("Id", userId),
//     //                 new Claim("BusinessId", businessId)
//     //             }, "mockUserAuth"));
//     //
//     //     var mockBusinessService = new Mock<IBusinessService>();
//     //     mockBusinessService.Setup(service => service.CreateBusiness(It.IsAny<CreateBusinessRequestDto>()))
//     //         .Returns(Task.FromResult<bool>(true));
//     //
//     //     BusinessController businessController = new BusinessController(mockBusinessService.Object);
//     //     businessController.ControllerContext = new ControllerContext() {
//     //         HttpContext = new DefaultHttpContext() { User = mockUser }
//     //     };
//     //
//     //     IActionResult response = await businessController.CreateBusiness(newBusinessForm);
//     //
//     //     Assert.IsType<OkResult>(response);
//     // }
//
//     [Fact]
//     public async void CreateBusiness_InvalidUserId_ReturnsUnauthorized() {
//         string userId = "";
//         string businessId = "BID123";
//         string BusinessName = "TestBusiness";
//
//         CreateBusinessForm newBusinessForm = new CreateBusinessForm(BusinessName);
//         var mockUser = new ClaimsPrincipal(
//             new ClaimsIdentity(
//                 new List<Claim>() {
//                     new Claim("Id", userId),
//                     new Claim("BusinessId", businessId)
//                 }, "mockUserAuth"));
//
//         var mockBusinessService = new Mock<IBusinessService>();
//         mockBusinessService.Setup(service => service.CreateBusiness(It.IsAny<CreateBusinessRequestDto>()))
//             .Returns(Task.FromResult<bool>(true));
//
//         BusinessController businessController = new BusinessController(mockBusinessService.Object);
//         businessController.ControllerContext = new ControllerContext() {
//             HttpContext = new DefaultHttpContext() { User = mockUser }
//         };
//
//         IActionResult response = await businessController.CreateBusiness(newBusinessForm);
//
//         Assert.IsType<UnauthorizedResult>(response);
//     }
// }