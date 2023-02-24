using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace instock_server_application.Users.Services; 

public interface IJwtService {
    public string CreateToken(string email);
}