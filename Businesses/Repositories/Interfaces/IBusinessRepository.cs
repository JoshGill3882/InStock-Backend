using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IBusinessRepository {
    Task<Dictionary<string, AttributeValue>> GetBusiness(BusinessDto businessDto);
    Task<BusinessDto> SaveNewBusiness(StoreBusinessDto businessToSave);
    Task<bool> DoesUserOwnABusiness(Guid userId);
}