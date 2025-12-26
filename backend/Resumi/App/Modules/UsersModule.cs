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
        IDomainService<AppUser> service,
        IDomainValidator<AppUser> validator,
        IRepository<AppUser> repository,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager, IUserMapper mapper)
        : base(service, validator, repository, userManager, roleManager)
    {
        Mapper = mapper;
    }
}
