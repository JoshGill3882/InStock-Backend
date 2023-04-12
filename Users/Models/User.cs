using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Shared.Dto;

namespace instock_server_application.Users.Models;

[DynamoDBTable("Users")]
public class User {
    [DynamoDBIgnore]
    public static string TableName = "Users";
    
    [DynamoDBHashKey]
    public string UserId { get; set; }
    [DynamoDBGlobalSecondaryIndexHashKey("Email")]
    public string Email { get; set; }
    [DynamoDBProperty]
    public string AccountStatus { get; set; }
    [DynamoDBProperty]
    public long CreationDate { get; set; }
    [DynamoDBProperty]
    public string FirstName { get; set; }
    [DynamoDBProperty]
    public string LastName { get; set; }
    [DynamoDBProperty]
    public string Password { get; set; }
    [DynamoDBProperty]
    public string Role { get; set; }
    [DynamoDBProperty]
    public string BusinessId { get; set; }
    [DynamoDBProperty]
    public string RefreshToken { get; set; }
    [DynamoDBProperty]
    public string RefreshTokenExpiry { get; set; }

    /// <summary>
    /// All Args Constructor
    /// </summary>
    /// <param name="userId"> User's ID</param>
    /// <param name="email"> User's Email </param>
    /// <param name="accountStatus"> User's Account </param>
    /// <param name="creationDate"> User's Creation </param>
    /// <param name="firstName"> User's First Name </param>
    /// <param name="lastName"> User's Last Name </param>
    /// <param name="password"> User's Password </param>
    /// <param name="role"> User's Role </param>
    /// <param name="businessId"> User's Linked Business </param>
    /// <param name="refreshToken"> user's Refresh Token </param>
    public User(string userId, string email, string accountStatus, long creationDate, string firstName, string lastName, string password, string role, string businessId, string refreshToken, string refreshTokenExpiry) {
        UserId = userId;
        Email = email;
        AccountStatus = accountStatus;
        CreationDate = creationDate;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Role = role;
        BusinessId = businessId;
        RefreshToken = refreshToken;
        RefreshTokenExpiry = refreshTokenExpiry;
    }

    public User() { }

    public User(UserDto userDto) {
        UserId = userDto.UserId;
        Email = userDto.Email;
        AccountStatus = userDto.AccountStatus;
        CreationDate = userDto.CreationDate;
        FirstName = userDto.FirstName;
        LastName = userDto.LastName;
        Password = userDto.Password;
        Role = userDto.Role;
        BusinessId = userDto.UserBusinessId;
        RefreshToken = userDto.RefreshToken;
        RefreshTokenExpiry = userDto.RefreshTokenExpiry;
    }
}