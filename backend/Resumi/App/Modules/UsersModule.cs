using Microsoft.AspNetCore.Identity;
using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class UsersModule : DomainModule<AppUser>
{
    public readonly IUserMapper Mapper;

    public UsersModule(
        IUserMapper mapper,
        IDomainService<AppUser> service,
        IDomainValidator<AppUser> validator,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IRepository<AppUser>? repository = null)
        : base(service, validator, userManager, roleManager, repository)
    {
        Mapper = mapper;
    }
}
