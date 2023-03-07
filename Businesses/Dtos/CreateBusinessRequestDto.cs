namespace instock_server_application.Businesses.Dtos; 

public class CreateBusinessRequestDto {
    public string BusinessName { get; }
    public string BusinessDescription { get; }
    public string UserId { get; }
    public string? UserCurrentBusinessId { get; }
    
    public CreateBusinessRequestDto(string businessName, string userId, string? userCurrentBusinessId, string businessDescription) {
        BusinessName = businessName;
        UserId = userId;
        UserCurrentBusinessId = userCurrentBusinessId;
        BusinessDescription = businessDescription;
    }
}