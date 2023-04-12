using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IBusinessRepository {
    Task<BusinessDto?> GetBusiness(ValidateBusinessIdDto validateBusinessIdDto);
    Task<BusinessDto> SaveNewBusiness(StoreBusinessDto businessToSave);
    Task<bool> DoesUserOwnABusiness(Guid userId);
    void UpdateBusinessDeviceTokens(BusinessDeviceKeysUpdateModel businessDeviceKeysUpdateModel);
}