using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UrlShortener.Domain.Primitives;

namespace UrlShortener.Persistence.Interceptors;

public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<IAuditableEntity>> entities = 
                dbContext
                    .ChangeTracker
                    .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                entity.Property(c => c.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
            }

            if (entity.State == EntityState.Modified)
            {
                entity.Property(c => c.ModifiedOnUtc).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
