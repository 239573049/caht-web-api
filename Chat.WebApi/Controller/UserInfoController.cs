using Chat.Application.Dto.User;
using Chat.Application.Services.User;
using Chat.WebCore.Base;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Controller;

public class UserInfoController : WebApiController
{
    private readonly IUserInfoService _userInfoService;
    private readonly IPrincipalAccessor _principalAccessor;
    public UserInfoController(
        IUserInfoService userInfoService,
        IPrincipalAccessor principalAccessor
        )
    {
        _userInfoService= userInfoService;
        _principalAccessor = principalAccessor;
    }
    /// <summary>
    /// 获取账号学习
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<UserInfoDto> GetUserInfo()
    {
        return await _userInfoService.GetUserInfo(_principalAccessor.UserId());
    }

}
