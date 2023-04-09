using instock_server_application.Users.Models;

namespace instock_server_application.Security.Dtos; 

public class RefreshTokenDto {
    public User User { get; }

    public RefreshTokenDto(User user) { User = user; }
}