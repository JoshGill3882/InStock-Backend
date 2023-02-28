using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace instock_server_application.Users.Models;

[DynamoDBTable("Users")]
public class User {
    [DynamoDBHashKey]
    public string UserId { get; set; }
    
    [DynamoDBProperty("Email")]
    public string Email { get; set; }
    
    [DynamoDBProperty("AccountStatus")]
    public string AccountStatus { get; set; }
    
    [DynamoDBProperty("CreationDate")]
    public int CreationDate { get; set; }
    
    [DynamoDBProperty("FirstName")]
    public string FirstName { get; set; }
    
    [DynamoDBProperty("LastName")]
    public string LastName { get; set; }
    
    [DynamoDBProperty("Password")]
    public string Password { get; set; }
    
    [DynamoDBProperty("Role")]
    public string Role { get; set; }
    
    [DynamoDBProperty("BusinessId")]
    public string BusinessId { get; set; }

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
    /// <param name="businessId"> User's Linked Businesses</param>
    public User(string userId, string email, string accountStatus, int creationDate, string firstName, string lastName, string password, string role, string businessId) {
        UserId = userId;
        Email = email;
        AccountStatus = accountStatus;
        CreationDate = creationDate;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Role = role;
        BusinessId = businessId;
    }
}