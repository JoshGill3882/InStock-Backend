using System.Net;
using System.Net.Mail;
using System.Text;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using instock_server_application.AwsS3.Repositories;
using instock_server_application.AwsS3.Repositories.Interfaces;
using instock_server_application.AwsS3.Services;
using instock_server_application.AwsS3.Services.Interfaces;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Email.Services;
using instock_server_application.Email.Services.Interfaces;
using instock_server_application.Security.Models;
using instock_server_application.Security.Services;
using instock_server_application.Security.Services.Interfaces;
using instock_server_application.Shared.Filters;
using instock_server_application.Users.Repositories;
using instock_server_application.Users.Repositories.Interfaces;
using instock_server_application.Users.Services;
using instock_server_application.Users.Services.Interfaces;
using instock_server_application.Util.Services;
using instock_server_application.Util.Services.Interfaces;
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
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>(c => new DynamoDBContext(client));

// AWS S3 Credential Setup
using var s3Client = new AmazonS3Client(
    builder.Configuration["AWS_S3_ACCESS_KEY"], 
    builder.Configuration["AWS_S3_SECRET_KEY"],
    RegionEndpoint.EUWest2
);
builder.Services.AddSingleton<IAmazonS3>(s3Client);

// Firebase setup
var firebaseParams = $@"{{
    ""type"": ""{builder.Configuration["FIREBASE_SECRET_KEY:type"]}"",
    ""project_id"": ""{builder.Configuration["FIREBASE_SECRET_KEY:project_id"]}"",
    ""private_key_id"": ""{builder.Configuration["FIREBASE_SECRET_KEY:private_key_id"]}"",
    ""private_key"": ""{builder.Configuration["FIREBASE_SECRET_KEY:private_key"]}"",
    ""client_email"": ""{builder.Configuration["FIREBASE_SECRET_KEY:client_email"]}"",
    ""client_id"": ""{builder.Configuration["FIREBASE_SECRET_KEY:client_id"]}"",
    ""auth_uri"": ""{builder.Configuration["FIREBASE_SECRET_KEY:auth_uri"]}"",
    ""token_uri"": ""{builder.Configuration["FIREBASE_SECRET_KEY:token_uri"]}"",
    ""auth_provider_x509_cert_url"": ""{builder.Configuration["FIREBASE_SECRET_KEY:auth_provider_x509_cert_url"]}"",
    ""client_x509_cert_url"": ""{builder.Configuration["FIREBASE_SECRET_KEY:client_x509_cert_url"]}"",
}}";

FirebaseApp.Create(
    new AppOptions() {
        Credential = GoogleCredential.FromJson(firebaseParams)
    }
);

// Security Services and Repositories
builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// User Services & Repositories
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserService, UserService>();

// Business Services & Repositories
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IItemRepo, ItemRepo>();
builder.Services.AddScoped<ICreateAccountService, CreateAccountService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemStockService, ItemStockService>();
builder.Services.AddScoped<IItemOrderService, ItemOrderService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IConnectionsService, ConnectionsService>();
builder.Services.AddScoped<IItemConnectionService, ItemConnectionService>();
builder.Services.AddScoped<IMilestoneService, MilestoneService>();
builder.Services.AddScoped<IMilestoneRepository, MilestoneRepository>();
builder.Services.AddScoped<IBusinessConnectionService, BusinessConnectionService>();

// Util Services & Repositories
builder.Services.AddScoped<IUtilService, UtilService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// AwsS3 Services & Repositories
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IStorageRepository, StorageRepository>();

// External Connection Polling Service
builder.Services.AddHostedService<ExternalShopPollingService>();

// Email Services & Repositories
var smtpClient = new SmtpClient("smtp.gmail.com", 587) {
    EnableSsl = true,
    UseDefaultCredentials = false,
    Credentials = new NetworkCredential(
        builder.Configuration["SENDER_EMAIL"],
        builder.Configuration["SENDER_PASSWORD"]
    )
};
builder.Services.AddSingleton<SmtpClient>(smtpClient);
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddControllers(options => {
    options.Filters.Add<GlobalExceptionFilter>();
    // options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
});

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