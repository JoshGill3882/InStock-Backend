using System.Text.RegularExpressions;
using instock_server_application.Users.Dtos;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;
using instock_server_application.Users.Services.Interfaces;

namespace instock_server_application.Users.Services; 

public class CreateAccountService : ICreateAccountService {
    private readonly IUserRepo _userRepo;
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public CreateAccountService(IUserRepo userRepo, IUserService userService, IPasswordService passwordService, IJwtService jwtService) {
        _userRepo = userRepo;
        _userService = userService;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Function for creating a new account, and either passing back a JWT or error message
    /// </summary>
    /// <param name="newAccountDto"> Passed in values from the controller </param>
    /// <returns> JWT or error message </returns>
    public async Task<string> CreateAccount(NewAccountDto newAccountDto) {
        // Validations
        if (!ValidateEmail(newAccountDto.Email)) { return "Email not valid"; }
        if (!ValidatePassword(newAccountDto.Password)) { return "Password not valid"; }
        if (DuplicateAccount(newAccountDto.Email)) { return "Duplicate account"; }

        User newUser = new User(
            _userService.GenerateUUID(),
            newAccountDto.Email,
            "Active",
            DateTime.UtcNow.Date.Ticks,
            newAccountDto.FirstName,
            newAccountDto.LastName,
            _passwordService.Encrypt(newAccountDto.Password),
            "Standard User",
            ""
        );
        
        _userRepo.Save(newUser);

        return _jwtService.CreateToken(newUser.Email, newUser.BusinessId);
    }

    /// <summary>
    /// Function for checking if an email is valid based on a Regex
    /// </summary>
    /// <param name="email"> Given email </param>
    /// <returns> true/false based on if the param matches the regex </returns>
    public bool ValidateEmail(string email) {
        return Regex.IsMatch(email, "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])");
    }

    /// <summary>
    /// Function for checking if a password is valid based on a Regex
    /// </summary>
    /// <param name="password"> Given email </param>
    /// <returns> true/false based on if the param matches the regex </returns>
    public bool ValidatePassword(string password) {
        return Regex.IsMatch(password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,32}$");
    }

    /// <summary>
    /// Function for checking if an email already exists in the database
    /// </summary>
    /// <param name="email"> The entered email to be checked </param>
    /// <returns> true/false if the email is found </returns>
    public bool DuplicateAccount(string email) {
        User user = _userRepo.GetByEmail(email).Result!;
        // If the user is null (there wasn't one found using the given email), return false 
        if (string.IsNullOrEmpty(user.UserId)) {
            return false;
        }
        // Otherwise return true
        return true;
    }
}