using Microsoft.AspNetCore.Identity;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class ExperiencesModule : DomainModule<Experience>
{
    public ExperiencesModule(
        IDomainService<Experience> service,
        IDomainValidator<Experience> validator,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IRepository<Experience>? repository = null)
        : base(service, validator, userManager, roleManager, repository)
    {
    }
}
