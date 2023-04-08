﻿namespace instock_server_application.Businesses.Dtos; 

public class CreateBusinessRequestDto {
    public string BusinessName { get; }
    public string BusinessDescription { get; }
    public string UserId { get; }
    public IFormFile? File { get; }
    
    public CreateBusinessRequestDto(string businessName, string userId, string businessDescription, IFormFile? file) {
        BusinessName = businessName;
        UserId = userId;
        BusinessDescription = businessDescription;
        File = file;
    }
}