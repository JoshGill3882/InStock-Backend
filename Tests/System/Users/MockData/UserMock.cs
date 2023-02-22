using instock_server_application.Users.Models;

namespace instock_server_application.tests.System.Users.MockData; 

public static class UserMock {
    public static User SingleUser() {
        return new User(
            "johnbarnes@gmail.com", 
            "Active", 
            1676904523, 
            "John", 
            "Barnes", 
            "Test123", 
            "StandardUser"
        );
    }
}