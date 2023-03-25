namespace instock_server_application.Shared.Dto; 

public class UserDto {
    public string UserId { get; }
    public string Email { get; }
    public string AccountStatus { get; }
    public long CreationDate { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Password { get; }
    public string Role { get; }
    public string UserBusinessId { get; }
    
    public UserDto(string userId, string userBusinessId) {
        UserId = userId;
        UserBusinessId = userBusinessId;
    }

    public UserDto(string userId, string email, string accountStatus, long creationDate, string firstName, string lastName, string password, string role, string userBusinessId) {
        UserId = userId;
        Email = email;
        AccountStatus = accountStatus;
        CreationDate = creationDate;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Role = role;
        UserBusinessId = userBusinessId;
    }
}