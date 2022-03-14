using Chat.Core.Base;
using Chat.Repository.Core;

namespace Chat.Repository.Repositorys;

public interface IMasterDbRepositoryBase<TEntity> : IRepository<MasterDbContext, TEntity, Guid> where TEntity : Entity<Guid>
{

}

public interface IMasterDbRepositoryBase<TEntity, TKey> : IRepository<MasterDbContext, TEntity, TKey> where TEntity : Entity<TKey> { }