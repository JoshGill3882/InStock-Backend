using System.Security.Authentication;
using System.Security.Claims;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Repositories;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Repositories.Validators;
using instock_server_application.Shared.Exceptions;

namespace instock_server_application.Businesses.Services; 

public class BusinessService : IBusinessService {
    private readonly IBusinessRepository _businessRepository;

    public BusinessService(IBusinessRepository businessRepository) {
        _businessRepository = businessRepository;
    }

    public async Task<bool> CreateBusiness(CreateBusinessRequestDto newBusinessRequest) {
        // Check if the user Id is valid
        if (string.IsNullOrEmpty(newBusinessRequest.UserId)) {
            throw new AuthenticationException();
        }
        
        // Check if the user has already got a business
        if (!string.IsNullOrEmpty(newBusinessRequest.UserCurrentBusinessId)) {
            throw new UserAlreadyOwnsBusinessException();
        }
        
        // Validate and clean the Business Name
        string businessName = BusinessValidator.ValidateBusinessName(newBusinessRequest.BusinessName);
        
        // Calling repo to create the business for the user
        StoreBusinessDto businessToSave =
            new StoreBusinessDto(businessName, newBusinessRequest.UserId);
        bool saveSuccess = await _businessRepository.SaveNewBusiness(businessToSave);

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