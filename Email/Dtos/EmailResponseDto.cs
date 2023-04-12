namespace instock_server_application.Email.Dtos; 

public class EmailResponseDto {
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = "";
}