using Chat.Core.Entities.Groups;

namespace Chat.Repository.Repositorys.Groups;


public interface IFriendRepository : IMasterDbRepositoryBase<Friend>
{
}
public class FriendRepository : MasterDbRepositoryBase<Friend>, IFriendRepository
{
    public FriendRepository(MasterDbContext dbContext) : base(dbContext)
    {
    }
}
