using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resumi.Domain.Models;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.Infra.Database.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<int>, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>(options), IDbTracker
{
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Degree> AcademicDegrees => Set<Degree>();
    public DbSet<Volunteership> VolunteerExperiences => Set<Volunteership>();
    public DbSet<Certificate> Certificates => Set<Certificate>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure AppUserRole to use the existing RoleId column (prevents RoleId1 shadow property)
        builder.Entity<AppUserRole>()
            .HasOne<IdentityRole<int>>()
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
    }

    public async Task<bool> CommitAsync()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<ITrackable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = null;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Property(e => e.CreatedAt).IsModified = false;
                    break;
            }
        }

        return await SaveChangesAsync() > 0;
    }
}
