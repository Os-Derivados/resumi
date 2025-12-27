using Microsoft.AspNetCore.Identity;
using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

public class ResumesModule : DomainModule<Resume>
{
    public ResumesModule(
        IDomainService<Resume> service,
        IDomainValidator<Resume> validator,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IRepository<Resume>? repository = null)
        : base(service, validator, userManager, roleManager, repository)
    {
    }
}