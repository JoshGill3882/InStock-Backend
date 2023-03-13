namespace instock_server_application.Users.Dtos; 

public class NewAccountDto {
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Password { get; }

    public NewAccountDto(string firstName, string lastName, string email, string password) {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }
}