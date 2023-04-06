using instock_server_application.AwsS3.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.AwsS3.Controllers; 

[ApiController]
public class StorageController : ControllerBase {
    private readonly IStorageService _service;

    public StorageController(IStorageService service) {
        _service = service;
    }

    [HttpPost]
    [Route("/upload")]
    public async Task<IActionResult> UploadFile(IFormFile file) {
        if (file.ContentType.StartsWith("image/")) {
            var result = await _service.UploadFileAsync(file);
            return Ok(result);
        } else {
            return BadRequest("You can only upload images");
        }
    }
    
}