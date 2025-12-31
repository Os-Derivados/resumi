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
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IRepository<Volunteership>? repository = null)
        : base(service, validator, userManager, roleManager, repository)
    {
    }
}