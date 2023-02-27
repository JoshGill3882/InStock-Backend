using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Repositories.Interfaces; 

public interface IBusinessRepository {
    bool CreateBusiness(BusinessModel newBusiness);
}