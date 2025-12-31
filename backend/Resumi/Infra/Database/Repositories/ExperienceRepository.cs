using Resumi.App.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Infra.Database.Repositories;

public class ExperienceRepository : Repository<Experience>
{
    public ExperienceRepository(AppDbContext context, ILogger<Repository<Experience>> logger)
        : base(context, logger)
    {
    }
}