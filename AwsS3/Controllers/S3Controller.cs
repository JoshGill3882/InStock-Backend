using instock_server_application.AwsS3.Models;
using instock_server_application.AwsS3.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.AwsS3.Controllers; 

[ApiController]
public class S3Controller : ControllerBase {
    private readonly IStorageService _service;

    public S3Controller(IStorageService service) {
        _service = service;
    }

    [HttpPost]
    [Route("/upload")]
    public async Task<IActionResult> UploadFile(IFormFile file) {
        await using var memoryStr = new MemoryStream();
        await file.CopyToAsync(memoryStr);

        var fileExt = Path.GetExtension(file.Name);
        var objName = $"{Guid.NewGuid()}.{fileExt}";

        var s3Obj = new S3Object() {
            BucketName = "instock-photos",
            InputStream = memoryStr,
            Name = objName
        };

        var result = await _service.UploadFileAsync(s3Obj);
        return Ok(result);
    }
    
}