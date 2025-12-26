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
        IRepository<Certificate> repository,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager
    )
        : base(service, validator, repository, userManager, roleManager)
    {
    }
}