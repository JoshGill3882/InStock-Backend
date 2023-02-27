namespace instock_server_application.Businesses.Dtos; 

public class UserDTO {
    private string UserId { get; set; } 
    private string UserBusinessId { get; set; }
    
    public UserDTO(string userId, string userBusinessId) {
        UserId = userId;
        UserBusinessId = userBusinessId;
    }
}