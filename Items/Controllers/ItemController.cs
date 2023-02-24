using instock_server_application.Items.Models;
using instock_server_application.Items.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Items.Controllers; 

[ApiController]
public class ItemController : ControllerBase {
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService) {
        _itemService = itemService;
    }

    [HttpGet]
    [Route("/getAllItems")]
    public async Task<IActionResult> GetAllItems(string businessId) {
        List<Dictionary<string, string>>? items = _itemService.GetItems(businessId).Result;
        
        if (items == null) {
            return NotFound("No Items Found");
        }

        return Ok(items);
    }
}