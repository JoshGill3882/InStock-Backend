namespace instock_server_application.Businesses.Dtos; 

public class StoreBusinessDto {
    public string BusinessName { get; }
    public string BusinessDescription { get; }
    public string UserId { get; }
    public string? ImageUrl { get; }
    public List<string> DeviceKeys { get; }

    public StoreBusinessDto(string businessName, string userId, string businessDescription, string? imageUrl, List<string> deviceKeys) {
        BusinessName = businessName;
        UserId = userId;
        BusinessDescription = businessDescription;
        ImageUrl = imageUrl;
        DeviceKeys = deviceKeys;
    }
}