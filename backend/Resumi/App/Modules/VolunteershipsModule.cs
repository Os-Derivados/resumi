using Microsoft.AspNetCore.Identity;
using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class VolunteershipsModule : DomainModule<Volunteership>
{
    public VolunteershipsModule(
        IDomainService<Volunteership> service,
        IDomainValidator<Volunteership> validator,
        IRepository<Volunteership> repository,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
        : base(service, validator, repository, userManager, roleManager)
    {
    }
}