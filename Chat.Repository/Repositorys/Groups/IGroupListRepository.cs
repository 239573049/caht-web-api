using Chat.Core.Entities.Groups;

namespace Chat.Repository.Repositorys.Groups;


public interface IGroupListRepository : IMasterDbRepositoryBase<GroupList>
{
}
public class GroupListRepository : MasterDbRepositoryBase<GroupList>, IGroupListRepository
{
    public GroupListRepository(MasterDbContext dbContext) : base(dbContext)
    {
    }
}
