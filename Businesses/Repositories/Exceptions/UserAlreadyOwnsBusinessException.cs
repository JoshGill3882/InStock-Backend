
namespace instock_server_application.Businesses.Repositories.Exceptions; 

// Thrown when the user is trying to create a business but they already own one
public class UserAlreadyOwnsBusinessException : Exception {
    public UserAlreadyOwnsBusinessException() : base(ErrorMessage()) {
    }

    private static string ErrorMessage() {
        return "A Business is already associated with this account.";
    }
}