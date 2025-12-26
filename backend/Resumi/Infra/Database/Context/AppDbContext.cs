using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.Infra.Constants;
using Resumi.Infra.Database.Interfaces;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Database.Context;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>, IDbTracker
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Degree> AcademicDegrees => Set<Degree>();
    public DbSet<Volunteership> VolunteerExperiences => Set<Volunteership>();
    public DbSet<Certificate> Certificates => Set<Certificate>();

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
