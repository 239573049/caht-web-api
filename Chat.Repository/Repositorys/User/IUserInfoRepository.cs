using Chat.Core.Entities.User;

namespace Chat.Repository.Repositorys.User;

public interface IUserInfoRepository:IMasterDbRepositoryBase<UserInfo>
{
}
public class UserInfoRepository : MasterDbRepositoryBase<UserInfo>, IUserInfoRepository
{
    public UserInfoRepository(MasterDbContext dbContext) : base(dbContext)
    {
    }
}
