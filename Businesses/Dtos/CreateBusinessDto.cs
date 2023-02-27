using instock_server_application.Businesses.Models;

namespace instock_server_application.Businesses.Dtos; 

public class CreateBusinessDto {
    public CreateBusinessDto(string businessName) {
        BusinessName = businessName;
    }

    public string BusinessName { get; }
}