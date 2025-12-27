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
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IRepository<Degree>? repository = null)
        : base(service, validator, userManager, roleManager, repository)
    {
    }
}
