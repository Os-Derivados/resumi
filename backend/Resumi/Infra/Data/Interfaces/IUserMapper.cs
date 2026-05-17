using Resumi.Api.Data.Models;

namespace Resumi.Infra.Data.Interfaces;

public interface IUserMapper : IEntityMapper<AppUser, UserModel, CreateUserModel, UpdateUserModel>;