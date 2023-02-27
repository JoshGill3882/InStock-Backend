namespace instock_server_application.Businesses.Dtos; 

public class UserDTO {
    private string UserId { get; } 
    private string UserBusinessId { get; }
    
    public UserDTO(string userId, string userBusinessId) {
        UserId = userId;
        UserBusinessId = userBusinessId;
    }
}