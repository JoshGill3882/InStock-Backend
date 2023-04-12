using instock_server_application.AwsS3.Dtos;
using instock_server_application.AwsS3.Models;

namespace instock_server_application.AwsS3.Repositories.Interfaces; 

public interface IStorageRepository {
    public Task<S3ResponseDto> UploadFileAsync(S3Model s3Model);
}