using System.Text.RegularExpressions;
using instock_server_application.AwsS3.Dtos;
using instock_server_application.AwsS3.Services.Interfaces;
using instock_server_application.Businesses.Dtos;
using instock_server_application.Businesses.Repositories.Interfaces;
using instock_server_application.Businesses.Services.Interfaces;
using instock_server_application.Shared.Dto;
using instock_server_application.Util.Services.Interfaces;

namespace instock_server_application.Businesses.Services; 

public class BusinessService : IBusinessService {
    private readonly IBusinessRepository _businessRepository;
    private readonly IUtilService _utilService;
    private readonly IStorageService _storageService;

    public BusinessService(IBusinessRepository businessRepository, IUtilService utilService, IStorageService storageService) {
        _businessRepository = businessRepository;
        _utilService = utilService;
        _storageService = storageService;
    }

    public async Task<BusinessDto> GetBusiness(ValidateBusinessIdDto validateBusinessIdDto) {
        ErrorNotification errorNote = new ErrorNotification();
        
        if (!_utilService.CheckUserBusinessId(validateBusinessIdDto.UserBusinessId, validateBusinessIdDto.BusinessId)) {
            // If the user doesn't have access, return "Unauthorized"
            errorNote.AddError("Unauthorized");
            return new BusinessDto(errorNote);
        }
        
        BusinessDto? responseItems = _businessRepository.GetBusiness(validateBusinessIdDto).Result;

        // User has access, but incorrect businessID or no business found
        if (responseItems == null) {
            // Return "Unauthorized"
            errorNote.AddError("Unauthorized");
            return new BusinessDto(errorNote);
        }
        
        return responseItems;
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
        
        if (newBusinessRequest.ImageFile != null) {
            _utilService.ValidateImageFileContentType(errorNotes, newBusinessRequest.ImageFile);
        }

        // If we've got errors then return the notes and not make a repo call
        if (errorNotes.HasErrors) {
            return new BusinessDto(errorNotes);
        }

        S3ResponseDto storageResponse = new S3ResponseDto();
        
        if (newBusinessRequest.ImageFile != null) {
            storageResponse = await _storageService.UploadFileAsync(new UploadFileRequestDto(newBusinessRequest.UserId, newBusinessRequest.ImageFile));
        }
        
        // Calling repo to create the business for the user
        StoreBusinessDto businessToSave = new StoreBusinessDto(
            newBusinessRequest.BusinessName,
            newBusinessRequest.UserId,
            newBusinessRequest.BusinessDescription,
            storageResponse.Message
        );
        
        BusinessDto createdBusiness = await _businessRepository.SaveNewBusiness(businessToSave);

        return createdBusiness;
    }
}