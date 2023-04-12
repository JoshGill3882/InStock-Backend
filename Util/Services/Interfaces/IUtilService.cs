using instock_server_application.Shared.Dto;

namespace instock_server_application.Util.Services.Interfaces; 

public interface IUtilService {
    public string GenerateUUID();
    public bool CheckUserBusinessId(string? userBusinessId, string businessIdToCheck);
    public void ValidateImageFileContentType(ErrorNotification errorNotes, IFormFile file);
}