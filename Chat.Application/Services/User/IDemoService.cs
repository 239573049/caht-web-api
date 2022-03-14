using Chat.Core.Entities.User;
using Chat.Repository;
using Chat.Repository.Repositorys;
using Management.Repository.Core;

namespace Chat.Application.Services.User;

public interface IDemoService
{
    string Get();
}
public class DemoService : BaseService<UserInfo>, IDemoService
{
    public DemoService(IUnitOfWork<MasterDbContext> unitOfWork, IMasterDbRepositoryBase<UserInfo> currentRepository) : base(unitOfWork, currentRepository)
    {
    }

    public string Get()
    {
        return "测试";
    }
}
