using instock_server_application.Security.Dtos;
using instock_server_application.Security.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;

namespace instock_server_application.Security.Services; 

public class RefreshTokenService : IRefreshTokenService {
    private readonly IUserRepo _userRepo;
    private static readonly Random Random = new ();

    public RefreshTokenService(IUserRepo userRepo) { _userRepo = userRepo; }
    
    public async Task<string> CreateToken(RefreshTokenDto refreshTokenDto) {
        // Get User Model from DTO
        User user = refreshTokenDto.User;
        
        // Create String List
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        // Generate Random String of length 500 based on characters
        string token = 
        user.RefreshToken = new string(
            Enumerable.Repeat(chars, 500)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray()
        );
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7).ToString();

    // Save User (with new token)
        _userRepo.Save(new UserDto(user));
        // Return the token
        return user.RefreshToken;
    }
}