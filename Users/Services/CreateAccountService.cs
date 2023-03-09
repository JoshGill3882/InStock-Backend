using System.Text.RegularExpressions;
using instock_server_application.Shared.Dto;
using instock_server_application.Shared.Services.Interfaces;
using instock_server_application.Users.Dtos;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;
using instock_server_application.Users.Services.Interfaces;

namespace instock_server_application.Users.Services; 

public class CreateAccountService : ICreateAccountService {
    private readonly IUserRepo _userRepo;
    private readonly IUtilService _utilService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public CreateAccountService(IUserRepo userRepo, IUtilService utilService, IPasswordService passwordService, IJwtService jwtService) {
        _userRepo = userRepo;
        _utilService = utilService;
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
        if (!ValidateFirstName(newAccountDto.FirstName)) { return "First Name not valid"; }
        if (!ValidateLastName(newAccountDto.LastName)) { return "Last Name not valid"; }
        if (!ValidateEmail(newAccountDto.Email)) { return "Email not valid"; }
        if (!ValidatePassword(newAccountDto.Password)) { return "Password not valid"; }
        if (DuplicateAccount(newAccountDto.Email)) { return "Duplicate account"; }

        UserDto newUser = new UserDto(
            _utilService.GenerateUUID(),
            newAccountDto.Email,
            "Active",
            DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            newAccountDto.FirstName,
            newAccountDto.LastName,
            _passwordService.Encrypt(newAccountDto.Password),
            "Standard User",
            ""
        );
        
        _userRepo.Save(newUser);

        return _jwtService.CreateToken(newUser.Email, newUser.UserBusinessId);
    }

    /// <summary>
    /// Function for checking if the User's entered First Name is null or empty
    /// </summary>
    /// <param name="firstName"> The User's entered First Name</param>
    /// <returns> True/False for if the User's First Name is null or empty </returns>
    private bool ValidateFirstName(string firstName) {
        return Regex.IsMatch(firstName, "^[A-Za-z][A-Za-z' -]{1,}$") | !string.IsNullOrEmpty(firstName);
    }
    /// <summary>
    /// Function for checking that the last name entered is not null or empty
    /// </summary>
    /// <param name="lastName"> The User's Entered Last Name </param>
    /// <returns> True/False depending on if it's empty </returns>
    private bool ValidateLastName(string lastName) {
        return Regex.IsMatch(lastName, "^[A-Za-z][A-Za-z' -]{1,}$") | !string.IsNullOrEmpty(lastName);
    }
    /// <summary>
    /// Function for checking if an email is valid based on a Regex
    /// </summary>
    /// <param name="email"> Given email </param>
    /// <returns> true/false based on if the param matches the regex </returns>
    private bool ValidateEmail(string email) {
        return Regex.IsMatch(email, "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\]).{1,320}$");
    }
    /// <summary>
    /// Function for checking if a password is valid based on a Regex
    /// </summary>
    /// <param name="password"> Given email </param>
    /// <returns> true/false based on if the param matches the regex </returns>
    private bool ValidatePassword(string password) {
        // Define regular expressions for each password rule
        Regex lengthRegex = new Regex("^.{8,32}$");
        Regex specialCharRegex = new Regex("[^a-zA-Z0-9]");
        Regex numberRegex = new Regex("[0-9]");
        Regex capitalLetterRegex = new Regex("[A-Z]");
        Regex lowercaseLetterRegex = new Regex("[a-z]");

        // Check if password meets all rules
        if (!lengthRegex.IsMatch(password) ||
            !specialCharRegex.IsMatch(password) ||
            !numberRegex.IsMatch(password) ||
            !capitalLetterRegex.IsMatch(password) ||
            !lowercaseLetterRegex.IsMatch(password)) {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Function for checking if an email already exists in the database
    /// </summary>
    /// <param name="email"> The entered email to be checked </param>
    /// <returns> true/false if the email is found </returns>
    private bool DuplicateAccount(string email) {
        User user = _userRepo.GetByEmail(email).Result!;
        // If the user is null (there wasn't one found using the given email), return false 
        if (string.IsNullOrEmpty(user.UserId)) {
            return false;
        }
        // Otherwise return true
        return true;
    }
}