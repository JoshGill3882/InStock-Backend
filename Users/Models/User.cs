using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Users.Models; 

[DynamoDBTable("Users")]
public class User {
    [DynamoDBHashKey("Email")]
    private string UserEmail { get; set; }
    [DynamoDBProperty("AccountStatus")]
    private string UserAccountStatus { get; set; }
    [DynamoDBProperty("CreationDate")]
    private int UserCreationDate { get; set; }
    [DynamoDBProperty("FirstName")]
    private string UserFirstName { get; set; }
    [DynamoDBProperty("LastName")]
    private string UserLastName { get; set; }
    [DynamoDBProperty("Password")]
    private string UserPassword { get; set; }
    [DynamoDBProperty("Role")]
    private string UserRole { get; set; }

    /// <summary>
    /// All Args Constructor
    /// </summary>
    /// <param name="userEmail"> User's Email </param>
    /// <param name="userAccountStatus"> User's Account Status </param>
    /// <param name="userCreationDate"> User's Creation Date </param>
    /// <param name="userFirstName"> User's First Name </param>
    /// <param name="userLastName"> User's Last Name </param>
    /// <param name="userPassword"> User's Password </param>
    /// <param name="userRole"> User's Role </param>
    public User(string userEmail, string userAccountStatus, int userCreationDate, string userFirstName, string userLastName, string userPassword, string userRole) {
        UserEmail = userEmail;
        UserAccountStatus = userAccountStatus;
        UserCreationDate = userCreationDate;
        UserFirstName = userFirstName;
        UserLastName = userLastName;
        UserPassword = userPassword;
        UserRole = userRole;
    }
}