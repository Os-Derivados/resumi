using Microsoft.AspNetCore.Identity;
using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class CertificatesModule : DomainModule<Certificate>
{
    public CertificatesModule(
        IDomainService<Certificate> service,
        IDomainValidator<Certificate> validator,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IRepository<Certificate>? repository = null)
        : base(service, validator, userManager, roleManager, repository)
    {
    }
}