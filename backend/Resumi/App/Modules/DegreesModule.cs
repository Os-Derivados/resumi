using Microsoft.AspNetCore.Identity;
using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class DegreesModule : DomainModule<Degree>
{
    public DegreesModule(
        IDomainService<Degree> service,
        IDomainValidator<Degree> validator,
        IRepository<Degree> repository,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager
    )
        : base(service, validator, repository, userManager, roleManager)
    {
    }
}
