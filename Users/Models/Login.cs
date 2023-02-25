namespace instock_server_application.Users.Models;

public class Login
{
    public string Email { get; set; }
    
    public string Password { get; set; }

    /// <summary>
    /// All Args Constructor
    /// </summary>
    /// <param name="email"> User's Email </param>
    /// <param name="password"> User's Password </param>
    public Login(string email,string password) {
        Email = email;
        Password = password;
    }
}