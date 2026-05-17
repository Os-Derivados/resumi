using Microsoft.AspNetCore.Identity;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class CertificatesModule : DomainModule<Certificate>
{
    public readonly ICertificateMapper Mapper;

    public CertificatesModule(
        IDomainService<Certificate> service,
        IDomainValidator<Certificate> validator,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        ICertificateMapper mapper,
        IRepository<Certificate>? repository = null)
        : base(service, validator, userManager, roleManager, repository)
    {
        Mapper = mapper;
    }
}