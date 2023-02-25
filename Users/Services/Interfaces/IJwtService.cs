using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using instock_server_application.Users.Models;
using Microsoft.IdentityModel.Tokens;

namespace instock_server_application.Users.Services; 

public interface IJwtService {
    public string CreateToken(User user);
}