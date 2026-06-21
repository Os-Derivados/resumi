using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Domain.Validators;

public class ExperienceValidator : IDomainValidator<Experience>
{
	public Result ValidateCreation(Experience? newExperience)
	{
		throw new NotImplementedException();
	}

	public Result ValidateUpdate(Experience? current, Experience? updated)
	{
		throw new NotImplementedException();
	}

	public Result ValidateDeletion(Experience? targetExperience)
	{
		throw new NotImplementedException();
	}
}