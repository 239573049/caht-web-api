using Microsoft.EntityFrameworkCore;

namespace Chat.Repository.Core;

public interface IUnitOfWork<TDbContext> where TDbContext : DbContext
{
    void BeginTransaction();
    int SaveChanges();
    Task<int> SaveChangesAsync();
    void CommitTransaction();
    void RollbackTransaction();
}