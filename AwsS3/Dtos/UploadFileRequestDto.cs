using instock_server_application.Shared.Dto;

namespace instock_server_application.AwsS3.Dtos; 

public class UploadFileRequestDto : DataTransferObjectSuperType {
    public string UserId { get; }
    public IFormFile File { get; }

    public UploadFileRequestDto(string userId, IFormFile file) {
        UserId = userId;
        File = file;
    }
}