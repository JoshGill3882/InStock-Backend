using instock_server_application.AwsS3.Dtos;
using instock_server_application.AwsS3.Models;

namespace instock_server_application.AwsS3.Services.Interfaces; 

public interface IStorageService {
    Task<S3ResponseDto> UploadFileAsync(S3Object s3Object);
}