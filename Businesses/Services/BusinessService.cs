using System.Text.RegularExpressions;
using Amazon.DynamoDBv2.Model;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Shared.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class BusinessService : IBusinessService {
    private readonly IBusinessRepository _businessRepository;
    private readonly IUtilService _utilService;

    public BusinessService(IBusinessRepository businessRepository, IUtilService utilService) {
        _businessRepository = businessRepository;
        _utilService = utilService;
    }

    public async Task<Dictionary<string, string>> GetBusiness(ValidateBusinessIdDto validateBusinessIdDto) {

        if (_utilService.CheckUserBusinessId(validateBusinessIdDto.UserBusinessId, validateBusinessIdDto.BusinessId)) {
            Dictionary<string, AttributeValue> responseItems = _businessRepository.GetBusiness(validateBusinessIdDto).Result;
            Dictionary<string, string> business = new();
                
            // User has access, but incorrect businessID or no business found
            if (responseItems.Count == 0) {
                // Return an empty list
                return business;
            }

            foreach (var data in responseItems) {
                business.Add(data.Key, data.Value.S);
            }

            return business;
        }
        
        // If the user doesn't have access, return "null"
        return null;
    }

    private void ValidateBusinessName(ErrorNotification errorNotes, string businessName) {
        // Business Name Variables
        const string errorKey = "businessName";
        const int businessNameMaxLength = 20;
        Regex businessNameRegex = new Regex(@"^[a-zA-Z0-9]+(\s+[a-zA-Z0-9]+)*$");

        if (string.IsNullOrEmpty(businessName)) {
            errorNotes.AddError(errorKey, "The business name cannot be empty.");
        }
        
        if (businessName.Length > businessNameMaxLength) {
            errorNotes.AddError(errorKey, $"The business name cannot exceed {businessNameMaxLength} characters.");
        }
        
        if (!businessNameRegex.IsMatch(businessName)) {
            errorNotes.AddError(errorKey, "The business name can only contain alphanumeric characters.");
        }
    }

    public async Task<BusinessDto> CreateBusiness(CreateBusinessRequestDto newBusinessRequest) {

        ErrorNotification errorNotes = new ErrorNotification();
        
        // Check if the user Id is valid, they should be validated by this point so throw exception
        if (string.IsNullOrEmpty(newBusinessRequest.UserId)) {
            throw new NullReferenceException("The UserId cannot be null or empty.");
        }
        
        // Check if the user has already got a business, this makes the following other validations meaningless so return
        if (await _businessRepository.DoesUserOwnABusiness(new Guid(newBusinessRequest.UserId))) {
            errorNotes.AddError("A Business is already associated with your account.");
            return new BusinessDto(errorNotes);
        }
        
        // Validate and clean the Business Name
        ValidateBusinessName(errorNotes, newBusinessRequest.BusinessName);

        // If we've got errors then return the notes and not make a repo call
        if (errorNotes.HasErrors) {
            return new BusinessDto(errorNotes);
        }
        
        // Calling repo to create the business for the user
        StoreBusinessDto businessToSave =
            new StoreBusinessDto(newBusinessRequest.BusinessName, newBusinessRequest.UserId, newBusinessRequest.BusinessDescription);
        
        BusinessDto createdBusiness = await _businessRepository.SaveNewBusiness(businessToSave);

        return createdBusiness;
    }
}