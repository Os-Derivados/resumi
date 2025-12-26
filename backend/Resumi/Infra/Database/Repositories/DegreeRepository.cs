using Resumi.App.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Infra.Database.Repositories;

public class DegreeRepository : Repository<Degree>
{
    public DegreeRepository(AppDbContext context, ILogger<Repository<Degree>> logger)
        : base(context, logger)
    {
    }
}