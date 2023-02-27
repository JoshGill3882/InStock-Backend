using instock_server_application.Businesses.Models;
using instock_server_application.Businesses.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Businesses.Controllers; 

[ApiController]
public class ItemController : ControllerBase {
    private readonly IItemService _itemService;
    private readonly IBusinessService _businessService;

    public ItemController(IItemService itemService, IBusinessService businessService) {
        _itemService = itemService;
        _businessService = businessService;
    }

    /// <summary>
    /// Function for getting all the items for a specific business, providing the currently logged in user has access
    /// </summary>
    /// <param name="businessId"> The BusinessID to get all the items for </param>
    /// <returns> List of all the Items found, or an error message with a 404 status code </returns>
    [HttpGet]
    [Route("/getAllItems")]
    public async Task<IActionResult> GetAllItems([FromBody] BusinessIdModel businessIdModel) {
        
        if (_businessService.CheckBusinessIdInJWT(User, businessIdModel.BusinessId)) {
            List<Dictionary<string, string>>? items = _itemService.GetItems(businessIdModel.BusinessId).Result;

            if (items == null) {
                return NotFound("No Items Found");
            }

            return Ok(items);
        }

        return NotFound("User Not In Business");
    }
}