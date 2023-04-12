namespace instock_server_application.Email.Objects; 

public class EmailResponseObject {
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = "";
}