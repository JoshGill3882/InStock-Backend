using System.Text;
using Amazon;
using Amazon.DynamoDBv2;
using instock_server_application.Businesses.Repositories;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string jwtSecretKey = builder.Configuration[JwtKey.EnvironmentKeyIndex + ":SecretKey"];
string jwtIssuer = builder.Configuration[JwtKey.EnvironmentKeyIndex + ":Issuer"];
string jwtAudience = builder.Configuration[JwtKey.EnvironmentKeyIndex + ":Audience"];

// JWT Bearer tokens
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o => {
    o.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(jwtSecretKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

// Environment Variables / Config
builder.Services.Configure<JwtKey>(builder.Configuration.GetSection(JwtKey.EnvironmentKeyIndex));

// AWS DynamoDB Credential Setup
var client = new AmazonDynamoDBClient(
    builder.Configuration["AWS_DYNAMO_DB_ACCESS_KEY"],
    builder.Configuration["AWS_DYNAMO_DB_SECRET_KEY"],
    RegionEndpoint.EUWest2
);
builder.Services.AddSingleton<IAmazonDynamoDB>(client);

// User Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserService, UserService>();

// Business Services & Repositories
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();