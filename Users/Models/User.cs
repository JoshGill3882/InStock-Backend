using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace instock_server_application.Users.Models;

public class User {
    [DynamoDBHashKey]
    public string UserId { get; set; }
    public string Email { get; set; }
    public string AccountStatus { get; set; }
    public int CreationDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public List<string> Businesses { get; set; }

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
    /// <param name="businesses"> User's Linked Businesses</param>
    public User(string userId, string email, string accountStatus, int creationDate, string firstName, string lastName, string password, string role, List<string> businesses) {
        UserId = userId;
        Email = email;
        AccountStatus = accountStatus;
        CreationDate = creationDate;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Role = role;
        Businesses = businesses;
    }
}