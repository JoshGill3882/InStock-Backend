using instock_server_application.AwsS3.Dtos;
using instock_server_application.AwsS3.Models;
using instock_server_application.AwsS3.Repositories.Interfaces;
using instock_server_application.AwsS3.Services.Interfaces;
using instock_server_application.Util.Services.Interfaces;

namespace instock_server_application.AwsS3.Services; 

public class StorageService : IStorageService {
    private readonly IStorageRepository _storageRepo;
    private readonly IUtilService _utilService;
    
    public StorageService(IStorageRepository storageRepo, IUtilService utilService) {
        _storageRepo = storageRepo;
        _utilService = utilService;
    }
    
    public async Task<S3ResponseDto> UploadFileAsync(UploadFileRequestDto uploadFileRequestDto) {
        
        // Check if the user Id is valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(uploadFileRequestDto.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        
        await using var memoryStr = new MemoryStream();
        await uploadFileRequestDto.File.CopyToAsync(memoryStr);

        var fileExt = Path.GetExtension(uploadFileRequestDto.File.FileName);
        var objName = $"{_utilService.GenerateUUID()}{fileExt}";
        var s3Model = new S3Model(objName, memoryStr, S3Model.S3BucketName);

        S3ResponseDto response = _storageRepo.UploadFileAsync(s3Model).Result;

        return response;
    }
}