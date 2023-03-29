using Amazon.DynamoDBv2.DataModel;
using instock_server_application.Shared.Dto;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;

namespace instock_server_application.Users.Repositories; 

public class UserRepo : IUserRepo {
    private readonly IDynamoDBContext _dynamoDbContext;

    public UserRepo(IDynamoDBContext dynamoDbContext) { _dynamoDbContext = dynamoDbContext; }

    public async Task<User> GetByEmail(string email) {
        User? user = _dynamoDbContext.QueryAsync<User>(
            email,
            new DynamoDBOperationConfig {IndexName = "Email"}
            ).GetRemainingAsync().Result.FirstOrDefault();

        return user ?? new User();
    }
    
    public async Task<User> GetByRefreshToken(string token) {
        User? user = _dynamoDbContext.QueryAsync<User>(
            token,
            new DynamoDBOperationConfig {IndexName = "RefreshToken"}
        ).GetRemainingAsync().Result.FirstOrDefault();

        return user ?? new User();
    }

    public async void Save(UserDto userDto) {
        User user = new User(userDto);
        await _dynamoDbContext.SaveAsync(user);
    }
}