using AutoMapper;
using Chat.Application.Dto.User;
using Chat.Application.Dto.WX;
using Chat.Core.DbEnum;
using Chat.Core.Entities.User;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Extension;
using Chat.Infrastructure.Helper;
using Chat.Repository;
using Chat.Repository.Repositorys;
using Management.Repository.Core;
using Microsoft.EntityFrameworkCore;
using XHHttpUtil;
namespace Chat.Application.Services.User;

public interface IUserInfoService
{
    /// <summary>
    /// 微信登录接口
    /// </summary>
    /// <param name="code"></param>
    /// <param name="name"></param>
    /// <param name="headPortrait"></param>
    /// <returns></returns>
    Task<UserInfoDto> WXLogin(string? code, string? name, string? headPortrait);
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserInfoDto> GetUserInfo(Guid id);
    /// <summary>
    /// 编辑用户信息
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<UserInfoDto> UpdateUserInfo(UserInfoDto user);
    /// <summary>
    /// 获取账号信息（无隐私信息）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserInfoDto> GetUserById(Guid id);
}
public class UserInfoService : BaseService<UserInfo>, IUserInfoService
{
    private readonly IMapper _mapper;

    public UserInfoService(
        IMapper mapper,
        IUnitOfWork<MasterDbContext> unitOfWork, 
        IMasterDbRepositoryBase<UserInfo> currentRepository) : base(unitOfWork, currentRepository)
    {
        _mapper = mapper;
    }

    public async Task<UserInfoDto> GetUserById(Guid id)
    {
        var data = await currentRepository.FindAll(a => a.Id == id)
                .FirstOrDefaultAsync();
        if (data == null) throw new BusinessLogicException("用户不存在或者已经被删除");
        data.Password = "";
        data.EMail = "";
        data.Mobile =null;
        data.WXOpenId = "";
        return _mapper.Map<UserInfoDto>(data);
    }

    public async Task<UserInfoDto> GetUserInfo(Guid id)
    {
        var data=await currentRepository.FindAll(a=>a.Id==id)
                .FirstOrDefaultAsync();
        if (data == null) throw new BusinessLogicException("用户不存在或者已经被删除");
        return _mapper.Map<UserInfoDto>(data);
    }

    public async Task<UserInfoDto> UpdateUserInfo(UserInfoDto user)
    {
        var data=await currentRepository.FindAll(a=>a.Id==user.Id).FirstOrDefaultAsync();
        if (data == null) throw new BusinessLogicException("用户不存在或者已经被删除");
        user.Statue=data.Statue;
        user.AccountNumber=data.AccountNumber;
        user.Password=data.Password;
        user.WXOpenId=data.WXOpenId;
        _mapper.Map(user,data);
        currentRepository.Update(data);
        await unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserInfoDto>(data);
    }

    public async Task<UserInfoDto> WXLogin(string? code, string? name, string? headPortrait)
    {
        var appid = AppSettings.App("appid");
        var secret = AppSettings.App("secret");
        var data = await HttpHelp.GetAsync<WxJscode2session>(string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&grant_type=authorization_code&js_code={2}&connect_redirect=1", appid, secret, code));
        if (data.Errcode != 0) throw new BusinessLogicException("获取用户账号失败");
        var userInfo = currentRepository.FirstOrDefault(a => a.WXOpenId == data.Openid);
        if(userInfo == null)
        {
            userInfo = new()
            {
                WXOpenId = data.Openid,
                ChatHead = headPortrait,
                Name = name,
                Statue = StatueEnum.Enable,
                AccountNumber = data.Openid,
                Sex = SexEnum.None,
                Password = "Aa123456".DESEncrypt(),
            };
            userInfo=await currentRepository.AddAsync(userInfo);
            await unitOfWork.SaveChangesAsync();
        }
        return _mapper.Map<UserInfoDto>(userInfo);
    }
}
