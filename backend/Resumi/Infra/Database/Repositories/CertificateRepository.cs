using Resumi.App.Data.Models;
using Resumi.Infra.Database.Context;

namespace Resumi.Infra.Database.Repositories;

public class CertificateRepository : Repository<Certificate>
{
    public CertificateRepository(AppDbContext context, ILogger<Repository<Certificate>> logger)
        : base(context, logger)
    {
    }
}