using Resumi.App.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Infra.Database.Repositories;

public class ResumeRepository : Repository<Resume>
{
    public ResumeRepository(AppDbContext context, ILogger<Repository<Resume>> logger) : base(context, logger)
    {
    }
}