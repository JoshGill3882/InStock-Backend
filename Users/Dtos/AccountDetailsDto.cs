namespace instock_server_application.Users.Dtos; 

public class AccountDetailsDto {
    public String FirstName { get; }
    public String LastName { get; }
    public String Email { get; }

    public AccountDetailsDto(string firstName, string lastName, string email) {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}