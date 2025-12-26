using Resumi.App.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Infra.Database.Repositories;

public class VolunteershipRepository : Repository<Volunteership>
{
    public VolunteershipRepository(AppDbContext context, ILogger<Repository<Volunteership>> logger) : base(context,
        logger)
    {
    }
}