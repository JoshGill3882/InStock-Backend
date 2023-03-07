namespace instock_server_application.Businesses.Dtos; 

public class CreateBusinessRequestDto {
    public string BusinessName { get; }
    public string UserId { get; }
    public string? UserCurrentBusinessId { get; }
    
    public CreateBusinessRequestDto(string businessName, string userId, string? userCurrentBusinessId) {
        BusinessName = businessName;
        UserId = userId;
        UserCurrentBusinessId = userCurrentBusinessId;
    }
}