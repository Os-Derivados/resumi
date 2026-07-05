using System.Linq.Expressions;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Infra.Constants;

namespace Resumi.Infra.Data.Projections;

using UserProjection = Expression<Func<AppUser, UserModel>>;

public static class UserProjections
{
	public static readonly UserProjection Basic = u => new UserModel
	{
		Id = u.Id,
		FullName = u.FullName,
		PhoneNumber = u.PhoneNumber,
		Email = u.Email!,
		IsAdmin = u.UserRoles!.AsQueryable().Any(ur => ur.Role!.Name == AuthConstants.AdminRole)
	};
}