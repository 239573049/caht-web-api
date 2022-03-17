using AutoMapper;
using Chat.Application.Dto.Groups;
using Chat.Application.Dto.User;
using Chat.Core.DbEnum;
using Chat.Core.Entities.Groups;
using Chat.Core.Entities.User;
using Chat.Infrastructure.Exceptions;
using Chat.Repository;
using Chat.Repository.Repositorys;
using Chat.Repository.Repositorys.Groups;
using Chat.Repository.Repositorys.User;
using Management.Repository.Core;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Services.Groups;

public interface IApplicationRecordService
{
    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="beAppliedId"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<bool> AddFriend(ApplicationRecordDto record);
    /// <summary>
    /// 处理好友申请
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    Task<(ApplicationRecordDto, Guid)> ApplyForDealWith(Guid id, bool isRefuse);
    /// <summary>
    /// 获取当前用户申请记录
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<ApplicationRecordDto>, int)> GetApplicationRecordPage(Guid userId, int pageNo = 1, int pageSize = 20);
}
public class ApplicationRecordService : BaseService<ApplicationRecord>, IApplicationRecordService
{
    private readonly IMapper _mapper;
    private readonly IFriendRepository _friendRepository;
    private readonly IUserInfoRepository _userInfoRepository;
    public ApplicationRecordService(
        IMapper mapper,
        IFriendRepository friendRepository,
        IUserInfoRepository userInfoRepository,
        IUnitOfWork<MasterDbContext> unitOfWork,
        IMasterDbRepositoryBase<ApplicationRecord> currentRepository
        ) : base(unitOfWork, currentRepository)
    {
        _mapper = mapper;
        _friendRepository = friendRepository;
        _userInfoRepository = userInfoRepository;
    }

    public async Task<bool> AddFriend(ApplicationRecordDto record)
    {
        if (await currentRepository.IsExist(a => a.ApplicantId == record.ApplicantId && a.BeAppliedId == a.BeAppliedId && a.FriendStatue == FriendStatueEnum.Applying))
            throw new BusinessLogicException("请勿重复发起好友申请");
        var applicationRecord = _mapper.Map<ApplicationRecord>(record);
        applicationRecord.FriendStatue = FriendStatueEnum.Applying;
        await currentRepository.AddAsync(applicationRecord);
        return (await unitOfWork.SaveChangesAsync()) > 0;
    }

    public async Task<(ApplicationRecordDto, Guid)> ApplyForDealWith(Guid id, bool isRefuse)
    {
        var data = await currentRepository.FirstOrDefaultAsync(a => a.Id == id);
        if (data == null) throw new BusinessLogicException("好友申请不存在");
        if (data.FriendStatue != FriendStatueEnum.Applying) throw new BusinessLogicException("当前好友申请已经处理");
        data.FriendStatue = isRefuse ? FriendStatueEnum.Refuse : FriendStatueEnum.Succeed;//拒绝同意处理
        unitOfWork.BeginTransaction();
        currentRepository.Update(data);
        var firend = new List<Friend>();
        var connectId = Guid.NewGuid();
        firend.Add(new Friend
        {
            ConnectId = connectId,
            BelongId = data.ApplicantId,
            FriendsId = data.ApplicantId,
        });
        firend.Add(new Friend
        {
            ConnectId = connectId,
            FriendsId = data.ApplicantId,
            BelongId = data.ApplicantId,
        });
        await _friendRepository.AddManyAsync(firend);
        unitOfWork.CommitTransaction();
        return (_mapper.Map<ApplicationRecordDto>(data), connectId);
    }

    public async Task<(List<ApplicationRecordDto>, int)> GetApplicationRecordPage(Guid userId, int pageNo = 1, int pageSize = 20)
    {
        var data = await currentRepository
            .GetPageAsync(a => a.ApplicantId == userId || a.BeAppliedId == userId, a => a.CreatedTime, pageNo, pageSize, false);
        var dto = _mapper.Map<List<ApplicationRecordDto>>(data.Item1);
        var ids = dto.Where(a => a.ApplicantId == userId).Select(a => a.BeAppliedId).ToList();
        ids.AddRange(dto.Where(a => a.BeAppliedId == userId).Select(a => a.ApplicantId).ToList());
        var users = await _userInfoRepository.FindAll(a => ids.Contains(a.Id)).ToListAsync();
        foreach (var d in dto)
        {
            Func<UserInfo, UserInfoDto> selector = new
                (a => new UserInfoDto { Id = a.Id, Name = a.Name, AccountNumber = a.AccountNumber, ChatHead = a.ChatHead, Description = a.Description });
            if (d.ApplicantId == userId)
            {
                var user = users.FindAll(a => a.Id == d.BeAppliedId)
                                                .Select(selector).FirstOrDefault();
                d.AddInformation = user;
            }
            else
            {
                var user = users.FindAll(a => a.Id == d.ApplicantId)
                                            .Select(selector).FirstOrDefault();
                d.AddInformation = user;
            }
        }
        return (dto, data.Item2);
    }
}
