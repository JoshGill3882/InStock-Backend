using instock_server_application.AwsS3.Dtos;
using instock_server_application.AwsS3.Models;
using instock_server_application.AwsS3.Repositories.Interfaces;
using instock_server_application.AwsS3.Services.Interfaces;

namespace instock_server_application.AwsS3.Services; 

public class StorageService : IStorageService {
    private readonly IStorageRepository _storageRepo;
    
    public StorageService(IStorageRepository storageRepo) {
        _storageRepo = storageRepo;
    }
    
    public async Task<S3ResponseDto> UploadFileAsync(IFormFile file) {
        await using var memoryStr = new MemoryStream();
        await file.CopyToAsync(memoryStr);

        var fileExt = Path.GetExtension(file.Name);
        var objName = $"{Guid.NewGuid()}.{fileExt}";
        var s3Model = new S3Model(objName, memoryStr);

        S3ResponseDto response = _storageRepo.UploadFileAsync(s3Model).Result;

        return response;
    }
}