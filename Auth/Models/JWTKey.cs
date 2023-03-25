namespace instock_server_application.Auth.Models; 

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0#bind-hierarchical-configuration-data-using-the-options-pattern
public class JwtKey {
    
    // The name of the environment variable that holds the JWT KEY (check appsettings.json)
    public const string EnvironmentKeyIndex = "jwt";
    
    public string SecretKey { get; set; } = String.Empty;
    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
}