namespace instock_server_application.Businesses.Dtos; 

public class StoreBusinessDto {
    public string BusinessName { get; }
    public string UserId { get; }

    public StoreBusinessDto(string businessName, string userId) {
        BusinessName = businessName;
        UserId = userId;
    }
}