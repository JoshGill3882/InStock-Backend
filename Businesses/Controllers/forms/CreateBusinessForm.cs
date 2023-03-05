using System.ComponentModel.DataAnnotations;

namespace instock_server_application.Businesses.Controllers.forms; 

public class CreateBusinessForm {
    
    [MaxLength(20)]
    public string BusinessName { get; }

    public CreateBusinessForm(string businessName) {
        BusinessName = businessName;
    }
}