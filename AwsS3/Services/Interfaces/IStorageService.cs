using instock_server_application.AwsS3.Dtos;

namespace instock_server_application.AwsS3.Services.Interfaces; 

public interface IStorageService {
    Task<S3ResponseDto> UploadFileAsync(UploadFileRequestDto uploadFileRequestDto);
    public S3ResponseDto GetFilePresignedUrl(string bucketName, string fileName);
}