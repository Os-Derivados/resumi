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
        IRepository<Resume> repository,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
        : base(service, validator, repository, userManager, roleManager)
    {
    }
}