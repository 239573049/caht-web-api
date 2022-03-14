using Chat.Application.Services.User;
using Chat.WebCore.Base;
using Chat.WebCore.JWT;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Controller;
/// <summary>
/// 登录模块
/// </summary>
public class LoginController : WebApiController
{
    private readonly IJwtService _jwtService;
    private readonly IUserInfoService _userInfoService;
    private readonly IPrincipalAccessor _principalAccessor;
    public LoginController(
        IUserInfoService userInfoService,
        IJwtService jwtService,
        IPrincipalAccessor principalAccessor
        )
    {
        _jwtService=jwtService;
        _userInfoService = userInfoService;
        _principalAccessor = principalAccessor;
    }
    /// <summary>
    /// 微信登录
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> WXLogin(string? code, string? name, string? headPortrait)
    {
        var user =await _userInfoService.WXLogin(code, name,headPortrait);
        var token = await _jwtService.CreateTokenAsync(user.Id);//jwt令牌生成
        return new OkObjectResult(new { token, user });
    }
}
