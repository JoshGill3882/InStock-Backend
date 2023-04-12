using instock_server_application.Shared.Dto;

namespace instock_server_application.AwsS3.Dtos; 

public class UploadFileRequestDto : DataTransferObjectSuperType {
    public string UserId { get; }
    public string BucketName { get; }
    public IFormFile File { get; }

    public UploadFileRequestDto(string userId, string bucketName, IFormFile file) {
        UserId = userId;
        BucketName = bucketName;
        File = file;
    }
}