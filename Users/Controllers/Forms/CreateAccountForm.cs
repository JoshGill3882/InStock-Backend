namespace instock_server_application.Users.Controllers.Forms; 

public class CreateAccountForm {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile? ImageFile { get; set; }

    public CreateAccountForm(string firstName, string lastName, string email, string password, IFormFile? imageFile) {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        ImageFile = imageFile;
    }
    
    public CreateAccountForm() {
        // Parameterless constructor required for model binding
    }
}