using System.Text;
using Amazon;
using Amazon.DynamoDBv2;
using instock_server_application.Users.Models;
using instock_server_application.Users.Services;
using instock_server_application.Users.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
string jwtSecretKey = config.GetSection(JwtKey.EnvironmentKeyIndex + ":SecretKey").Value;
string jwtIssuer = config.GetSection(JwtKey.EnvironmentKeyIndex + ":Issuer").Value;
string jwtAudience = config.GetSection(JwtKey.EnvironmentKeyIndex + ":Audience").Value;

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

// Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

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