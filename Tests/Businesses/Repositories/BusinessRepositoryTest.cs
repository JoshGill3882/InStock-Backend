// using System.Runtime.CompilerServices;
// using Amazon.DynamoDBv2.DataModel;
// using instock_server_application.Businesses.Dtos;
// using instock_server_application.Businesses.Models;
// using instock_server_application.Businesses.Repositories;
// using instock_server_application.Users.Models;
// using Moq;
// using Xunit;
//
// namespace instock_server_application.Tests.Businesses.Repositories; 
//
// public class BusinessRepositoryTest {
//
//     [Fact]
//     // public async void CreateBusiness_ValidDetails_SavesObject() {
//     //     string businessName = "";
//     //     string userId = "";
//     //
//     //     var mockUserModel = new Mock<User>(userId, "", "", "", "", "", "", "", "BID123");
//     //     StoreBusinessDto storeBusinessDto = new StoreBusinessDto(businessName, userId);
//     //     var mockDynamoDbContext = new Mock<IDynamoDBContext>();
//     //     mockDynamoDbContext.Setup(context => context.LoadAsync<User>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
//     //         .Returns(Task.FromResult(mockUserModel.Object));
//     //     mockDynamoDbContext.Setup(context => context.SaveAsync(It.IsAny<BusinessModel>(), It.IsAny<CancellationToken>()))
//     //         .Returns(Task.FromResult(true));
//     //     mockDynamoDbContext.Setup(context => context.SaveAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
//     //         .Returns(Task.FromResult(true));
//     //     
//     //     BusinessRepository businessRepository = new BusinessRepository(mockDynamoDbContext.Object);
//     //
//     //     bool result = await businessRepository.SaveNewBusiness(storeBusinessDto);
//     //     
//     //     Assert.True(result);
//     // }
//
//     [Fact]
//     public void CreateBusiness_InvalidDetails_DoesntSaveObject() {
//         throw new NotImplementedException();
//     }
// }