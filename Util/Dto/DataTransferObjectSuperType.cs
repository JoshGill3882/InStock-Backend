namespace instock_server_application.Shared.Dto; 

// Martin Fowler
// https://martinfowler.com/eaaDev/Notification.html#:~:text=class%20DataTransferObject
public class DataTransferObjectSuperType {

    public ErrorNotification ErrorNotification { get; set; } = new ErrorNotification();

    public DataTransferObjectSuperType() {
    }

    public DataTransferObjectSuperType(ErrorNotification errorNotes) {
        ErrorNotification = errorNotes;
    }
    
    
}