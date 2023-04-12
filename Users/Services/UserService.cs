using instock_server_application.AwsS3.Services.Interfaces;
using instock_server_application.Users.Dtos;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;
using instock_server_application.Users.Services.Interfaces;

namespace instock_server_application.Users.Services; 

public class UserService : IUserService {
    private readonly IUserRepo _userRepo;
    private readonly IStorageService _storageService;

    public UserService(IUserRepo userRepo, IStorageService storageService) {
        _userRepo = userRepo;
        _storageService = storageService;
    }
    
    /// <summary>
    /// Function for getting a User's data, given an email
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <returns> Returns User's Data, or "null" if the User is not found </returns>
    public async Task<User?> FindUserByEmail(string email) {
        User? userDetails = _userRepo.GetByEmail(email).Result;

        // If there was no email found using the Repo method then there is no user using that email
        if (userDetails == null) {
            return null;
        }
        return userDetails;
    }
    
    public async Task<AccountDetailsDto> GetUser(string email) {
        User userDetails = await FindUserByEmail(email);
        
        string imageUrl = userDetails.ImageFilename != null 
            ? _storageService.GetFilePresignedUrl("instock-profile-pictures", userDetails.ImageFilename).Message 
            : "";

        AccountDetailsDto accountDetailsDto = new AccountDetailsDto(
            firstName: userDetails.FirstName,
            lastName: userDetails.LastName,
            email: userDetails.Email,
            imageUrl: imageUrl
        );
        
        return accountDetailsDto;
    }
}