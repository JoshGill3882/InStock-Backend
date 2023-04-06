using instock_server_application.Security.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace instock_server_application.Security.Controllers; 

[ApiController]
[Route("/refresh")]
public class RefreshTokenController : ControllerBase {
    private readonly IAccessTokenService _accessTokenService;

    public RefreshTokenController(IAccessTokenService accessTokenService) {
        _accessTokenService = accessTokenService;
    }

    [HttpGet]
    [Route("{refreshToken}")]
    public async Task<IActionResult> CreateAccessFromRefresh([FromRoute] string refreshToken) {
        var result = _accessTokenService.RefreshToAccess(refreshToken);

        return result.Equals("Refresh Token Invalid") ? Unauthorized() : Ok(result);
    }
}