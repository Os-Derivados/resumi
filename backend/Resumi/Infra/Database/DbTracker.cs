using Microsoft.EntityFrameworkCore;
using Resumi.Infra.Database.Context;

namespace Resumi.Infra.Database;

public class DbTracker
{
    private readonly AppDbContext _dbContext;

    public DbTracker(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync()
    {
        var trackedEntries = _dbContext.ChangeTracker.Entries().Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        foreach (var entry in trackedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                    break;
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}