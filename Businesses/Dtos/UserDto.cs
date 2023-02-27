namespace instock_server_application.Businesses.Dtos; 

public class UserDto {
    public string UserId { get; } 
    public string UserBusinessId { get; }
    
    public UserDto(string userId, string userBusinessId) {
        UserId = userId;
        UserBusinessId = userBusinessId;
    }
}