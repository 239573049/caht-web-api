using Chat.Core.Entities.Groups;

namespace Chat.Repository.Repositorys.Groups;

public interface IApplicationRecordRepository : IMasterDbRepositoryBase<ApplicationRecord>
{
}
public class ApplicationRecordRepository : MasterDbRepositoryBase<ApplicationRecord>, IApplicationRecordRepository
{
    public ApplicationRecordRepository(MasterDbContext dbContext) : base(dbContext)
    {
    }
}

