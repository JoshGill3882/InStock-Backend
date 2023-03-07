using System.Collections;

namespace instock_server_application.Shared.Dto; 

// Martin Fowler
// https://martinfowler.com/eaaDev/Notification.html#:~:text=class%20Notification
public class ErrorNotification {
    public IList Errors { get; set; } = new ArrayList();

    public bool HasErrors {
        get { return 0 != Errors.Count; }
    }

    // For holding predefined errors in inheriting DTOs
    private class Error {
        private string message;

        public Error(string message) {
            this.message = message;
        }
    }
}