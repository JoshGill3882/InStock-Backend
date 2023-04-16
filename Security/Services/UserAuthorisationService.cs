using instock_server_application.Businesses.Dtos;

namespace instock_server_application.Security.Services; 

public class UserAuthorisationService {
    
    public static bool UserCanEditBusiness(UserAuthorisationDto userAuthorisationDto, string businessId) {
        // Check if the user and business Ids are valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(userAuthorisationDto.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(userAuthorisationDto.UserBusinessId)) {
            throw new NullReferenceException("The BusinessId cannot be null or empty.");
        }
        
        // Return if the user BusinessId matches the BusinessId they are trying to edit
        return userAuthorisationDto.UserBusinessId.Equals(userAuthorisationDto.UserBusinessId);
    }
    
    
    public static bool UserCanGetBusinessItems(UserAuthorisationDto userAuthorisationDto, string businessId) {
        // Currently only those who can edit the items can get the items, so lets use the same method
        return UserCanEditBusiness(userAuthorisationDto, businessId);
    }
    
}