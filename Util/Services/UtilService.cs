using instock_server_application.Shared.Dto;
using instock_server_application.Util.Services.Interfaces;

namespace instock_server_application.Util.Services; 

public class UtilService: IUtilService {
    public string GenerateUUID() {
        return Guid.NewGuid().ToString();
    }

    public bool CheckUserBusinessId(string? userBusinessId, string businessIdToCheck) {
        return !string.IsNullOrEmpty(userBusinessId) & businessIdToCheck.Equals(userBusinessId);
    }
    
    public void ValidateImageFileContentType(ErrorNotification errorNotes, IFormFile file) {
        const string errorKey = "fileType";
        if (!file.ContentType.StartsWith("image/")) {
            errorNotes.AddError(errorKey, "You can only upload images.");
        }
    }
}