using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class UserValidator : IDomainValidator<AppUser>
{
	public Result ValidateCreation(AppUser? newEntity)
	{
		throw new NotImplementedException();
	}

	public Result ValidateUpdate(AppUser? current, AppUser? updated)
	{
		throw new NotImplementedException();
	}

	public Result ValidateDeletion(AppUser? targetEntity)
	{
		throw new NotImplementedException();
	}
}