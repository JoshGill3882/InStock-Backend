using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;
using instock_server_application.Users.Services.Interfaces;

namespace instock_server_application.Users.Services; 

public class UserService : IUserService {
    private readonly IUserRepo _userRepo;

    public UserService(IUserRepo userRepo) {
        _userRepo = userRepo;
    }
    
    /// <summary>
    /// Function for getting a User's data, given an email
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <returns> Returns User's Data, or "null" if the User is not found </returns>
    public async Task<User?> FindUserByEmail(string email) {
        var result = _userRepo.GetUser(email).Result;
        
        // If the Email does not contain the key "Email", return null
        // Means the given email was not found in the Database
        if (result == null) {
            return null;
        }

        List<AttributeValue> AWSBusinesses = result["Businesses"].L;
        List<string> businesses = new();
        foreach (var item in AWSBusinesses) {
            businesses.Add(item.S);
        }
        
        
        var userDetails = new User(
            result["UserId"].S,
            result["Email"].S,
            result["AccountStatus"].S,
            int.Parse(result["CreationDate"].N),
            result["FirstName"].S,
            result["LastName"].S,
            result["Password"].S,
            result["Role"].S,
            businesses
        );
        return userDetails;
    }
}