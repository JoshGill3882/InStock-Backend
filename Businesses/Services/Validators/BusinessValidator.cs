using System.Text.RegularExpressions;
using instock_server_application.Shared.Exceptions;

namespace instock_server_application.Businesses.Repositories.Validators; 

public static class BusinessValidator {

    // Business Name Variables
    public static int BusinessName_MaxLength = 10;
    public static Regex BusinessName_Regex = new Regex(@"^[a-zA-Z0-9]+(\s+[a-zA-Z0-9]+)*$");
    
    // Check the Business Name is valid
    public static string ValidateBusinessName(string businessNameToValidate) {

        Dictionary<string, string> errors = new Dictionary<string, string>();

        string businessName = businessNameToValidate.Trim();

        // Check if the business name is empty
        if (string.IsNullOrEmpty(businessName)) {
            errors["length"] = "The business name cannot be empty.";
        }
        
        // Check if the business is within the character limit of 20 (same as Etsy & Shopify)
        if (businessName.Length > BusinessName_MaxLength) {
            errors["length"] = $"The business name cannot exceed {BusinessName_MaxLength} characters.";
        }
        
        if (!BusinessName_Regex.IsMatch(businessName)) {
            errors["character"] = "The business name can only contain alphanumeric characters.";
        }
            
        if (errors.Count > 0) {
            throw new InvalidBusinessNameException(errors);
        }

        return businessName;
    }

}