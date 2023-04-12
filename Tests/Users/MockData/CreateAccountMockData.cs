using instock_server_application.Users.Controllers.Forms;
using instock_server_application.Users.Models;

namespace instock_server_application.Tests.Users.MockData; 

public static class CreateAccountMockData {
    public static User SampleUser() { 
        return new User(
            "6d1c2a8b-462d-4906-9693-e1f0d7b592d4",
            "johnbarnes@gmail.com", 
            "Active", 
            1676904523, 
            "John", 
            "Barnes", 
            "s0m3encrypt3dpa55w0rd", 
            "Standard User",
            "",
            "RefreshToken",
            "90Days",
            "https://image.png"
        );
    }

    public static CreateAccountForm SampleDto(string firstName, string lastName, string email, string password) {
        return new CreateAccountForm(
            firstName,
            lastName,
            email,
            password,
            null
        );
    }

    public static User EmptyUser() {
        return new User();
    }
}