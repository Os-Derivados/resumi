using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resumi.App.Data.Models;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.Infra.Database.Context;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>, IDbTracker
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

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

    	/// <summary>
	/// Configura as regras de negócio e validações para as entidades do banco de dados.
	/// </summary>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Validações do Resume
		modelBuilder.Entity<Resume>(entity =>
		{
			entity.Property(r => r.Title)
				.IsRequired()
				.HasMaxLength(255);

			entity.Property(r => r.Summary)
				.HasMaxLength(1000);

			// Um Resume pode ter márias experiências
			entity.HasMany(r => r.Experiences)
				.WithOne(e => e.Resume)
				.OnDelete(DeleteBehavior.Cascade);

			// Um Resume pode ter várias formações acadêmicas
			entity.HasMany(r => r.AcademicDegrees)
				.WithOne(a => a.Resume)
				.OnDelete(DeleteBehavior.Cascade);
		});

		// Validações do Experience
		modelBuilder.Entity<Experience>(entity =>
		{
			entity.Property(e => e.Position)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(e => e.Company)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(e => e.StartDate)
				.IsRequired();

			// Validação: Se não está "em andamento", EndDate é obrigatória
			entity.HasCheckConstraint(
				"CK_Experience_DateRange",
				"([EndDate] IS NULL OR [EndDate] >= [StartDate])");
		});

		// Validações do AcademicDegree
		modelBuilder.Entity<AcademicDegree>(entity =>
		{
			entity.Property(a => a.School)
				.IsRequired()
				.HasMaxLength(200);

			entity.Property(a => a.FieldOfStudy)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(a => a.StartDate)
				.IsRequired();
		});
	}
}
