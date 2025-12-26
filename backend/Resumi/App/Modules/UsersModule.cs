using Microsoft.AspNetCore.Identity;
using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class UsersModule : DomainModule<AppUser>
{
    public UsersModule(
        IDomainService<AppUser> service,
        IDomainValidator<AppUser> validator,
        IRepository<AppUser> repository,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager
    )
        : base(service, validator, repository, userManager, roleManager)
    {
    }
}
