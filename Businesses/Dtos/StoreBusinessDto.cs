namespace instock_server_application.Businesses.Dtos; 

public class StoreBusinessDto {
    public string BusinessName { get; }
    public string BusinessDescription { get; }
    public string UserId { get; }

    public StoreBusinessDto(string businessName, string userId, string businessDescription) {
        BusinessName = businessName;
        UserId = userId;
        BusinessDescription = businessDescription;
    }
}