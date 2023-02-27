using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories.Interfaces;

namespace instock_server_application.Businesses.Repositories; 

public class BusinessRepository : IBusinessRepository {

    private readonly IAmazonDynamoDB _client;

    public BusinessRepository(IAmazonDynamoDB client) {
        _client = client;
    }

    public bool CreateBusiness(BusinessModel newBusiness) {
        
        // Database Validation?
        
        // TODO We need to look at how we are storing database data
        return true;
    }
    
}