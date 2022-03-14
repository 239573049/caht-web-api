using Chat.WebCore.Base;
using Chat.WebCore.JWT;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Controller;

public class LoginController : WebApiController
{
    private readonly IJwtService _jwtService;
    private readonly IPrincipalAccessor _principalAccessor;
    public LoginController(
        IJwtService jwtService,
        IPrincipalAccessor principalAccessor
        )
    {
        _jwtService=jwtService;
        _principalAccessor = principalAccessor;
    }
    /// <summary>
    /// 微信登录
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> WXLogin(string code)
    {
        var token=await _jwtService.CreateTokenAsync(code);
        return new OkObjectResult(new { token });
    }
}
