﻿using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class BusinessDto : DataTransferObjectSuperType {
    public string BusinessId { get; }
    public string BusinessName { get; }
    public string BusinessDescription { get; }
    public string BusinessOwnerId { get; }
    public string? ImageUrl { get; }

    public BusinessDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public BusinessDto(string businessId, string businessName, string businessOwnerId, string businessDescription, string? imageUrl) {
        BusinessId = businessId;
        BusinessName = businessName;
        BusinessOwnerId = businessOwnerId;
        BusinessDescription = businessDescription;
        ImageUrl = imageUrl;
    }
}