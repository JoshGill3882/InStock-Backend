using Amazon.DynamoDBv2.DataModel;

namespace instock_server_application.Users.Models
{
    public class Users {
        [DynamoDBHashKey]
        public string Email { get; set; }
        public string AccountStatus { get; set; }
        public int CreationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public Users(string email, string accountStatus, int creationDate, string firstName, string lastName, string password, string role) {
            Email = email;
            AccountStatus = accountStatus;
            CreationDate = creationDate;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Role = role;
        }
    }
}