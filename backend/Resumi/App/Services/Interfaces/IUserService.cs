using Resumi.App.Data.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Interfaces;

public interface IUserService : IDomainService<AppUser>
{
    Task<Result<AppUser>> CreateAsync(AppUser? newEntity, string password);
}