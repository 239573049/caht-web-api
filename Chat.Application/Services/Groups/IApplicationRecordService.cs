using AutoMapper;
using Chat.Application.Dto.Groups;
using Chat.Application.Dto.User;
using Chat.Core.DbEnum;
using Chat.Core.Entities.Groups;
using Chat.Infrastructure.Exceptions;
using Chat.Repository;
using Chat.Repository.Repositorys;
using Chat.Repository.Repositorys.Groups;
using Management.Repository.Core;

namespace Chat.Application.Services.Groups;

public interface IApplicationRecordService
{
    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="beAppliedId"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<bool> AddFriend(ApplicationRecord record);
    /// <summary>
    /// 处理好友申请
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    Task<(ApplicationRecordDto,Guid)> ApplyForDealWith(Guid id, bool isRefuse);
}
public class ApplicationRecordService : BaseService<ApplicationRecord>, IApplicationRecordService
{
    private readonly IMapper _mapper;
    private readonly IFriendRepository _friendRepository;
    public ApplicationRecordService(
        IMapper mapper,
        IFriendRepository friendRepository,
        IUnitOfWork<MasterDbContext> unitOfWork, 
        IMasterDbRepositoryBase<ApplicationRecord> currentRepository
        ) : base(unitOfWork, currentRepository)
    {
        _mapper = mapper;
        _friendRepository = friendRepository;
    }

    public async Task<bool> AddFriend(ApplicationRecord record)
    {
        if (await currentRepository.IsExist(a => a.ApplicantId == record.ApplicantId && a.BeAppliedId == a.BeAppliedId && a.FriendStatue == Core.DbEnum.FriendStatueEnum.Applying))
            throw new BusinessLogicException("请勿重复发起好友申请");
        var applicationRecord=_mapper.Map<ApplicationRecord>(record);
        applicationRecord.FriendStatue = Core.DbEnum.FriendStatueEnum.Applying;
        await currentRepository.AddAsync(applicationRecord);
        return (await unitOfWork.SaveChangesAsync())>0;
    }

    public async Task<(ApplicationRecordDto,Guid)> ApplyForDealWith(Guid id,bool isRefuse)
    {
        var data=await currentRepository.FirstOrDefaultAsync(a=>a.Id == id);
        if (data == null) throw new BusinessLogicException("好友申请不存在");
        if (data.FriendStatue != FriendStatueEnum.Applying) throw new BusinessLogicException("当前好友申请已经处理");
        data.FriendStatue=isRefuse ? FriendStatueEnum.Refuse: FriendStatueEnum.Succeed;//拒绝同意处理
        unitOfWork.BeginTransaction();
        currentRepository.Update(data);
        var firend = new List<Friend>();
        var connectId=Guid.NewGuid();
        firend.Add(new Friend
        {
            ConnectId=connectId,
            BelongId = data.ApplicantId,
            FriendsId = data.ApplicantId,
        }); 
        firend.Add(new Friend
        {
            ConnectId =connectId,
            FriendsId = data.ApplicantId,
            BelongId= data.ApplicantId,
        });
        await _friendRepository.AddManyAsync(firend);
        unitOfWork.CommitTransaction();
        return (_mapper.Map<ApplicationRecordDto>(data),connectId);
    }
}
