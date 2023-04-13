namespace instock_server_application.Users.Dtos; 

public class NewAccountDto {
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Password { get; }
    public IFormFile? ImageFile { get; }

    public NewAccountDto(string firstName, string lastName, string email, string password, IFormFile? imageFile) {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        ImageFile = imageFile;
    }
}