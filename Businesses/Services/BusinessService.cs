using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Services; 

public class BusinessService : IBusinessService {

    public bool CreateBusiness(string userId, CreateBusinessDto newBusiness) {
        
        return true;
    }
    
}