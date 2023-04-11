namespace instock_server_application.Businesses.Dtos; 

public class CreateBusinessRequestDto {
    public string BusinessName { get; }
    public string BusinessDescription { get; }
    public string UserId { get; }
    public IFormFile? ImageFile { get; }
    public List<string> DeviceKeys { get; }
    
    public CreateBusinessRequestDto(string businessName, string userId, string businessDescription, IFormFile? imageFile, List<string> deviceKeys) {
        BusinessName = businessName;
        UserId = userId;
        BusinessDescription = businessDescription;
        ImageFile = imageFile;
        DeviceKeys = deviceKeys;
    }
}