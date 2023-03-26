using instock_server_application.Users.Models;

namespace instock_server_application.Tests.Users.MockData; 

public static class UserMock {
    public static User SingleUser() {
        return new User(
            "6d1c2a8b-462d-4906-9693-e1f0d7b592d4",
            "johnbarnes@gmail.com", 
            "Active", 
            1676904523, 
            "John", 
            "Barnes", 
            "s0m3encrypt3dpa55w0rd", 
            "StandardUser",
            "2a36f726-b3a2-11ed-afa1-0242ac120002",
            ""
        );
    }
}