using System.Collections;

namespace instock_server_application.Shared.Dto; 

// Martin Fowler
// https://martinfowler.com/eaaDev/Notification.html#:~:text=class%20Notification
public class ErrorNotification {
    public Dictionary<string, IList> Errors { get; } = new Dictionary<string, IList>();

    public bool HasErrors {
        get { return 0 != Errors.Count; }
    }

    public void AddError(string errorMessage) {
        if (!Errors.ContainsKey("otherErrors")) {
            Errors.Add("otherErrors", new ArrayList());
        }
        Errors["otherErrors"].Add(errorMessage);
    }
    
    public void AddError(string errorKey, string errorMessage) {
        if (!Errors.ContainsKey(errorKey)) {
            Errors.Add(errorKey, new ArrayList());
        }
        Errors[errorKey].Add(errorMessage);
    }

    // For holding predefined errors in inheriting DTOs
    private class Error {
        private string message;

        public Error(string message) {
            this.message = message;
        }
    }
}