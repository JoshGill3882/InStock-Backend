using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using instock_server_application.AwsS3.Dtos;
using instock_server_application.AwsS3.Models;
using instock_server_application.AwsS3.Repositories.Interfaces;

namespace instock_server_application.AwsS3.Repositories; 

public class StorageRepository : IStorageRepository {
    private readonly IAmazonS3 _client;

    public StorageRepository(IAmazonS3 client) {
        _client = client;
    }

    public async Task<S3ResponseDto> UploadFileAsync(S3Model s3Model) {
        var response = new S3ResponseDto();

        try {
            var uploadRequest = new TransferUtilityUploadRequest() {
                InputStream = s3Model.InputStream,
                Key = s3Model.Name,
                BucketName = s3Model.BucketName,
                CannedACL = S3CannedACL.NoACL
            };

            var transferUtility = new TransferUtility(_client);
            await transferUtility.UploadAsync(uploadRequest);

            response.StatusCode = 200;
            response.Message = s3Model.Name;
        }
        catch (AmazonS3Exception e) {
            response.StatusCode = (int)e.StatusCode;
            response.Message = e.Message;
        }
        catch (Exception e) {
            response.StatusCode = 500;
            response.Message = e.Message;
        }

        return response;
    }
    
    public S3ResponseDto GetFilePresignedUrl(string bucketName, string fileName) {
        var responseDto = new S3ResponseDto();
       
        var request = new GetPreSignedUrlRequest {
            BucketName = bucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        responseDto.StatusCode = 200;
        responseDto.Message = _client.GetPreSignedURL(request);
    
        return responseDto;
    }
}