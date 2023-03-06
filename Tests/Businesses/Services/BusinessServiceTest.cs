using System.Diagnostics;
using System.Security.Authentication;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services;
using instock_server_application.Shared.Exceptions;
using Moq;
using Xunit;

namespace instock_server_application.Tests.Businesses.Services; 

public class BusinessServiceTest {
    
    [Fact]
    public async Task CreateBusiness_InvalidUser_ReturnsException() {
        const string userId = "";
        const string? currentBusinessId = "";
        const string newBusinessName = "TestBusiness";
        
        CreateBusinessRequestDto newBusinessRequest = new CreateBusinessRequestDto(newBusinessName, userId, currentBusinessId);
        
        var mockBusinessRepository = new Mock<IBusinessRepository>();
        mockBusinessRepository.Setup(repo => repo.SaveNewBusiness(It.IsAny<StoreBusinessDto>())).Returns(Task.FromResult(true));

        BusinessService businessService = new BusinessService(mockBusinessRepository.Object);

        await Assert.ThrowsAsync<AuthenticationException>(() => businessService.CreateBusiness(newBusinessRequest));
    }
    
    [Fact]
    public async void CreateBusiness_ExceedBusinessNameLength_ReturnsException() {
        const string userId = "UID123";
        const string? currentBusinessId = "";
        const string newBusinessName = "TestBusinessThatIsVeryLongAndThatTypeThing";
        
        CreateBusinessRequestDto newBusinessRequest = new CreateBusinessRequestDto(newBusinessName, userId, currentBusinessId);
        
        var mockBusinessRepository = new Mock<IBusinessRepository>();
        mockBusinessRepository.Setup(repo => repo.SaveNewBusiness(It.IsAny<StoreBusinessDto>())).Returns(Task.FromResult(true));

        BusinessService businessService = new BusinessService(mockBusinessRepository.Object);

        await Assert.ThrowsAsync<BusinessNameExceedsMaxLengthException>(() => businessService.CreateBusiness(newBusinessRequest));
    }
    
    [Fact]
    public async void CreateBusiness_EmptyBusinessName_ReturnsException() {
        const string userId = "UID123";
        const string? currentBusinessId = "";
        const string newBusinessName = "";
        
        CreateBusinessRequestDto newBusinessRequest = new CreateBusinessRequestDto(newBusinessName, userId, currentBusinessId);
        
        var mockBusinessRepository = new Mock<IBusinessRepository>();
        mockBusinessRepository.Setup(repo => repo.SaveNewBusiness(It.IsAny<StoreBusinessDto>())).Returns(Task.FromResult(true));

        BusinessService businessService = new BusinessService(mockBusinessRepository.Object);

        await Assert.ThrowsAsync<BusinessNameEmptyException>(() => businessService.CreateBusiness(newBusinessRequest));
    }
    
    [Fact]
    public async void CreateBusiness_UserOwnsBusiness_ReturnsException() {
        const string userId = "UID123";
        const string? currentBusinessId = "BID12345";
        const string newBusinessName = "TestBusiness";
        
        CreateBusinessRequestDto newBusinessRequest = new CreateBusinessRequestDto(newBusinessName, userId, currentBusinessId);
        
        var mockBusinessRepository = new Mock<IBusinessRepository>();
        mockBusinessRepository.Setup(repo => repo.SaveNewBusiness(It.IsAny<StoreBusinessDto>())).Returns(Task.FromResult(true));

        BusinessService businessService = new BusinessService(mockBusinessRepository.Object);

        await Assert.ThrowsAsync<UserAlreadyOwnsBusinessException>(() => businessService.CreateBusiness(newBusinessRequest));
    }
    
    [Fact]
    public async void CreateBusiness_ValidDetails_ReturnsTrue() {
        const string userId = "ID123";
        const string? currentBusinessId = "";
        const string newBusinessName = "TestBusiness";
        
        CreateBusinessRequestDto newBusinessRequest = new CreateBusinessRequestDto(newBusinessName, userId, currentBusinessId);
        
        var mockBusinessRepository = new Mock<IBusinessRepository>();
        mockBusinessRepository.Setup(repo => repo.SaveNewBusiness(It.IsAny<StoreBusinessDto>())).Returns(Task.FromResult(true));

        BusinessService businessService = new BusinessService(mockBusinessRepository.Object);

        bool result = await businessService.CreateBusiness(newBusinessRequest);
        
        Assert.True(result);
    }
    
}