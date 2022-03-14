using Chat.Core.Base;
using Chat.WebCore.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Chat.Repository.Core;

public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly IPrincipalAccessor _principalAccessor;

    public UnitOfWork(TDbContext dbContext, IPrincipalAccessor principalAccessor)
    {
        _dbContext = dbContext
                     ?? throw new ArgumentNullException($"db context nameof{nameof(dbContext)} is null");
        _principalAccessor = principalAccessor;
    }

    public void BeginTransaction()
    {
        _dbContext.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        ApplyChangeConventions();
        try
        {
            _dbContext.SaveChanges();
            _dbContext.Database.CommitTransaction();
        }
        catch (Exception)
        {
            _dbContext.Database.RollbackTransaction();
            throw;
        }
    }

    public void RollbackTransaction()
    {
        _dbContext.Database.RollbackTransaction();
    }

    public async Task<int> SaveChangesAsync()
    {
        ApplyChangeConventions();
        return await _dbContext.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        ApplyChangeConventions();
        return _dbContext.SaveChanges();
    }

    protected void ApplyChangeConventions()
    {
        _dbContext.ChangeTracker.DetectChanges();
        var entities = _dbContext.ChangeTracker.Entries().ToList();
        foreach (var entry in entities)
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    SetDelete(entry);
                    break;
                case EntityState.Modified:
                    SetModification(entry.Entity);
                    break;
                case EntityState.Added:
                    SetCreation(entry.Entity);
                    break;
                default:
                    break;
            }
        }
    }

    private void SetCreation(object entityObj)
    {
        if (entityObj is not IHaveCreation)
        {
            return;
        }

        var entity = (IHaveCreation)entityObj;
        var userId = _principalAccessor.ID;
        entity.CreatedTime = DateTime.Now;
        if (userId != Guid.Empty)
        {
            entity.CreatedBy = userId;
        }
    }

    private void SetModification(object entityObj)
    {
        if (entityObj is not IHaveModification)
        {
            return;
        }
        var entity = (IHaveModification)entityObj;
        var userId = _principalAccessor.ID;
        entity.ModifiedTime = DateTime.Now;
        if (userId != Guid.Empty)
        {
            entity.ModifiedBy = userId;
        }
    }


    private void SetDelete(EntityEntry entry)
    {
        var entityObj = entry.Entity;
        if (entityObj is not IHaveDeletion)
        {
            return;
        }

        var entity = (IHaveDeletion)entityObj;
        entity.IsDeleted = true;
        entry.State = EntityState.Modified;
        var userId = _principalAccessor.ID;
        if (userId != Guid.Empty)
        {
            entity.DeletedBy = userId;
        }
        entity.DeletedTime = DateTime.Now;
    }
}