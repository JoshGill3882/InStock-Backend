using instock_server_application.Users.Dtos;
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
            ""
        );
    }

    public static NewAccountDto SampleDto(string firstName, string lastName, string email, string password) {
        return new NewAccountDto (
            firstName,
            lastName,
            email,
            password
        );
    }

    public static User EmptyUser() {
        return new User();
    }
}