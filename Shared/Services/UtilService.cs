using instock_server_application.Shared.Services.Interfaces;

namespace instock_server_application.Shared.Services; 

public class UtilService: IUtilService {
    public string GenerateUUID() {
        return Guid.NewGuid().ToString();
    }
}