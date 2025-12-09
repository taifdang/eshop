using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Web;

namespace Infrastructure.Data.Interceptors;

// ref: https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserProdvider _currentUser;

    public AuditableEntityInterceptor(ICurrentUserProdvider currentUser)
    {
        _currentUser = currentUser;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        OnBeforeSaving(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        OnBeforeSaving(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    // ref: https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
    // ref: https://www.meziantou.net/entity-framework-core-soft-delete-using-query-filters.htm
    public void OnBeforeSaving(DbContext? context)
    {
        try
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<IAggregate>())
            {
                var isAuditable = entry.Entity.GetType().IsAssignableTo(typeof(IAggregate));
                var userId = _currentUser.GetCurrentUserId();

                if (isAuditable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedBy = userId;
                            entry.Entity.CreatedAt = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            entry.Entity.LastModifiedBy = userId;
                            entry.Entity.LastModified = DateTime.Now;
                            entry.Entity.Version++;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.Entity.LastModifiedBy = userId;
                            entry.Entity.LastModified = DateTime.Now;
                            entry.Entity.IsDeleted = true;
                            entry.Entity.Version++;
                            break;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception("try for find IAggregate", ex);
        }
    }
}

