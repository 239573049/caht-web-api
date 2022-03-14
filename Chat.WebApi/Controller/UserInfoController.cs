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
    /// 获取账号信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<UserInfoDto> GetUserInfo()
    {
        return await _userInfoService.GetUserInfo(_principalAccessor.UserId());
    }
    /// <summary>
    /// 编辑账号信息
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<UserInfoDto> UpdateUserInfo(UserInfoDto user)
    {
        return await _userInfoService.UpdateUserInfo(user);
    }
}
