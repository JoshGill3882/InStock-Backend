using instock_server_application.Shared.Dto;
using instock_server_application.Users.Models;

namespace instock_server_application.Users.Repositories.Interfaces; 

public interface IUserRepo {
    public Task<User> GetByEmail(string email);
    public Task<User> GetByRefreshToken(string token);
    public void Save(UserDto userDto);
}