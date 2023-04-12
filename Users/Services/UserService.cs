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
        // TODO - Change this to use an instance of UserRepo
        User? userDetails = _userRepo.GetByEmail(email).Result;

        // If there was no email found using the Repo method then there is no user using that email
        if (userDetails == null) {
            return null;
        }
        return userDetails;
    }
}