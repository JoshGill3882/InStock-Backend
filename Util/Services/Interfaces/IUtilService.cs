namespace instock_server_application.Shared.Services.Interfaces; 

public interface IUtilService {
    public string GenerateUUID();
    public bool CheckUserBusinessId(string? userBusinessId, string businessIdToCheck);
}