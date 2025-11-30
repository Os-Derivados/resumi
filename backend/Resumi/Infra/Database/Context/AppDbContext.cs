using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resumi.App.Data.Models;

namespace Resumi.Infra.Database.Context;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<AcademicDegree> AcademicDegrees => Set<AcademicDegree>();
    public DbSet<VolunteerExperience> VolunteerExperiences => Set<VolunteerExperience>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
}
