﻿using Amazon.DynamoDBv2.DataModel;
using instock_server_application.Users.Models;
using instock_server_application.Users.Repositories.Interfaces;

namespace instock_server_application.Users.Repositories; 

public class UserRepo : IUserRepo {
    private readonly IDynamoDBContext _dynamoDbContext;

    public UserRepo(IDynamoDBContext dynamoDbContext) { _dynamoDbContext = dynamoDbContext; }

    public async Task<User?> GetByEmail(string email) {
        return _dynamoDbContext.QueryAsync<User>(
            email,
            new DynamoDBOperationConfig {IndexName = "Email"}
            ).GetRemainingAsync().Result.FirstOrDefault();
    }

    public async void Save(User user) {
        await _dynamoDbContext.SaveAsync(user);
    }
}