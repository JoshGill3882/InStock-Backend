using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class ItemOrderDto : DataTransferObjectSuperType {
    public int AmountOrdered { get; }
    public DateTime DateTimeAdded { get; }
    
    public const string USER_UNAUTHORISED_ERROR = "You are not authorized to edit this business.";

    public ItemOrderDto(ErrorNotification errorNotes) : base(errorNotes) {
    }

    public ItemOrderDto(int amountOrdered, DateTime dateTimeAdded) {
        AmountOrdered = amountOrdered;
        DateTimeAdded = dateTimeAdded;
    }
}