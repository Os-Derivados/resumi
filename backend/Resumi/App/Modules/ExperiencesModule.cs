using Microsoft.AspNetCore.Identity;
using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class ExperiencesModule : DomainModule<Experience>
{
    public ExperiencesModule(
        IDomainService<Experience> service,
        IDomainValidator<Experience> validator,
        IRepository<Experience> repository,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager
    )
        : base(service, validator, repository, userManager, roleManager)
    {
    }
}
