using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories;
using instock_server_application.Businesses.Repositories.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class BusinessService : IBusinessService {
    private IBusinessRepository _businessRepository;

    public BusinessService(IBusinessRepository businessRepository) {
        _businessRepository = businessRepository;
    }

    public async Task<bool> CreateBusiness(UserDto userDto, CreateBusinessDto newBusiness) {
        // Check if the user Id is valid
        if (string.IsNullOrEmpty(userDto.UserId)) {
            return false; // Invalid UserId
        }
        
        // Check if the user has already got a business
        if (!string.IsNullOrEmpty(userDto.UserBusinessId)) {
            return false; // Already has a business
        }
        
        // Check if the business is within the character limit of 20 (same as Etsy & Shopify)
        if (userDto.UserBusinessId.Length > 20) {
            return false; // Business Name too long
        }
        
        // Calling repo to create the business for the user
        bool saveSuccess = await _businessRepository.CreateBusiness(userDto.UserId, newBusiness);

        return saveSuccess;
    }

    /// <summary>
    /// Function for checking if a given BusinessId is contained within a User's Claims
    /// </summary>
    /// <param name="userDto">The users details</param>
    /// <param name="idToCheck"> The ID to check for </param>
    /// <returns> True/False depending if the ID is found </returns>
    public bool CheckBusinessIdInJWT(UserDto userDto, string idToCheck) {
        // Return if the business Id matches or not
        return userDto.UserBusinessId.Equals(idToCheck);
    }
}