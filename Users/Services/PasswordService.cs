namespace instock_server_application.Users.Services; 

/// <summary>
/// Service for everything to do with Password Security
/// Methods used provided by BCrypt for .NET - https://github.com/BcryptNet/bcrypt.net
/// </summary>
public class PasswordService {
    /// <summary>
    /// Method for verifying that a entered String matches a given Hash
    /// </summary>
    /// <param name="plaintext"> User Entered String </param>
    /// <param name="encrypted"> Hash to check </param>
    /// <returns> True/False depending if the Password matches </returns>
    public bool Verify(string plaintext, string encrypted) {
        return BCrypt.Net.BCrypt.Verify(plaintext, encrypted);
    }

    /// <summary>
    /// Method for encrypting an entered String
    /// </summary>
    /// <param name="plaintext"> String to be encrypted </param>
    /// <returns> Encrypted String </returns>
    public string Encrypt(string plaintext) {
        return BCrypt.Net.BCrypt.HashPassword(plaintext);
    }
}