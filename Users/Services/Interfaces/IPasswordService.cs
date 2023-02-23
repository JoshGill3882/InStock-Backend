namespace instock_server_application.Users.Services.Interfaces; 

public interface IPasswordService {
    public bool Verify(string password, string encrypted);
    public string Encrypt(string plainText);
}