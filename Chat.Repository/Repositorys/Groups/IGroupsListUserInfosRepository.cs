using Chat.Core.Entities.Groups;

namespace Chat.Repository.Repositorys.Groups;

public interface IGroupsListUserInfosRepository : IMasterDbRepositoryBase<GroupsListUserInfos>
{
}
public class GroupsListUserInfosRepository : MasterDbRepositoryBase<GroupsListUserInfos>, IGroupsListUserInfosRepository
{
    public GroupsListUserInfosRepository(MasterDbContext dbContext) : base(dbContext)
    {
    }
}

