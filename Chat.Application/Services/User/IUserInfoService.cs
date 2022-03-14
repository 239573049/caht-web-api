using AutoMapper;
using Chat.Application.Dto.WX;
using Chat.Core.DbEnum;
using Chat.Core.Entities.User;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Extension;
using Chat.Infrastructure.Helper;
using Chat.Repository;
using Chat.Repository.Repositorys;
using Management.Repository.Core;
using XHHttpUtil;
namespace Chat.Application.Services.User;

public interface IUserInfoService
{
    Task<UserInfo> WXLogin(string? code, string? name, string? headPortrait);
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

    public async Task<UserInfo> WXLogin(string? code, string? name, string? headPortrait)
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
                Password = StringHelper.GetString(6).DESEncrypt(),
            };
            userInfo=await currentRepository.AddAsync(userInfo);
        }
        return _mapper.Map<UserInfo>(userInfo);
    }
}
