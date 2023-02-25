using instock_server_application.Users.Models;

namespace instock_server_application.Tests.Users.MockData; 

public static class UserMock {
    public static User SingleUser() {
        return new User(
            null,
            "johnbarnes@gmail.com", 
            "Active", 
            1676904523, 
            "John", 
            "Barnes", 
            "s0m3encrypt3dpa55w0rd", 
            "StandardUser",
            null
        );
    }
}